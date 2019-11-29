/** @addtogroup vlUnitySDK
 *  @{
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;

/// <summary>
///  Behaviour, which enables the user to use the GameObjects mesh for tracking. NOTE: This behaviour is considered as BETA.
///  Please do not alter this file unless you know what you are doing - it is considered to be changed and optimized in future versions.
///  
/// USAGE: You can add this behaviour to an UnityObject which might contain a mesh. You can allow the transmission of the model inside your definition
/// using the useForTracking flag. If you enable the occluder property, it will be rendered for occlusion of objects while tracking. 
/// If you want to disable the visibility of the object in Unity, disable the mesh.
///
/// </summary>
[System.Serializable]
public class VLModelTrackableBehaviour : MonoBehaviour
{
    private struct ModelData
    {
        public Mesh mesh;
        public Matrix4x4 transform;
    }

    [Serializable]
    private struct ModelTransform
    {
        public float[] t;
        public float[] s;
        public float[] q;
    }

    [Serializable]
    private struct ModelDataDescriptor
    {
        public string name;
        public string type;
        public bool enabled;
        public bool occluder;

        public ModelTransform transform;
        public BinaryDataDescriptor[] subModels;
    }

    [Serializable]
    private struct BinaryDataDescriptor
    {
        public string name;
        public int binaryOffset;
        public int updateCount;
        public int vertexCount;
        public int triangleIndexCount;
        public int normalCount;
    }

    [Serializable]
    private class AddModelDataCommandDescription
    {
        public string name;
        public List<ModelDataDescriptor> models;

        public AddModelDataCommandDescription()
        {
            this.name = "addModelData";
            this.models = new List<ModelDataDescriptor>();
        }
    }

     private Queue<GCHandle> gcHandleQueue = new Queue<GCHandle>();


    /// <summary>
    ///  Releases all allocated memory which has been passed to PushJsonAndBinaryCommand before.
    ///  Since the memory is pinned during the execution, this function MUST be called by the callback.
    ///  THIS FUNCTION IS CONSIDERED AS BETA AND MAY BE MATTER OF CHANGE.
    /// </summary>
    private void FreeBinaryMemory()
    {
        while (gcHandleQueue.Count != 0) 
        {
            gcHandleQueue.Dequeue().Free();
        }
    }


    /// <summary>
    ///  Releases the first allocated memory which has been passed to gcHandleQueue.
    ///  Since the memory is pinned during the execution, this function MUST be called by the callback.
    ///  THIS FUNCTION IS CONSIDERED AS BETA AND MAY BE MATTER OF CHANGE.
    /// </summary>
    private void FreeNextBinaryMemoryBlock()
    {
        if (gcHandleQueue.Count != 0) 
        {
            gcHandleQueue.Dequeue().Free();
        }
    }


    private bool internalUseForTracking = true;
    private bool internalOccluder = false;

    // React to changes on these variables

    /// <summary>
    ///  ModelTracker Behaviour used in this tracking scenario. 
    ///  This behaviour tries to find the corresponding VLModelTrackerBehaviour automatically if not defined.
    /// </summary>
    protected VLModelTrackerBehaviour_v1 modelTrackerBehaviour;

    /// <summary>
    ///  HoloLens Tracker Behaviour used in this tracking scenario. 
    ///  This behaviour tries to find the corresponding VLHoloLensTrackerBehaviour automatically if not defined.
    /// </summary>
    protected VLHoloLensTrackerBehaviour holoLensTrackerBehaviour;

    /// <summary>
    ///  Enable/Disable this property in order to use/not use all models in this object for tracking. 
    /// </summary>
    public bool useForTracking = true;

    /// <summary>
    ///  Enable/Disable this property in order to use/not use all models in this object for occlusion tracking. 
    /// </summary>
    public bool occluder = false;

    private GCHandle gcHandle;

    /// <summary>
    ///  List of model descriptions, which are currently used for tracking.
    /// </summary>
    public List<VLModelDeserializationResult> modelDescriptions;

    /// <summary>
    /// Update count, which will be propagated to the tracking system.
    /// </summary>
    private int globalUpdateCount = 0;
    /// <summary>
    /// Temporary variable, which counts, how many bytes are used for the 
    /// binary data.
    /// </summary>
    private int binaryOffset = 0;
    /// <summary>
    /// Temporary list of all meshes, which will be added to the binary data.
    /// </summary>
    private List<ModelData> modelData = new List<ModelData>();
    

    /// <summary>
    ///  Resets the modelDescriptions to the currently used ones.
    ///  This function is called by the callback after models have been added,
    ///  updated or removed.
    /// </summary>
    private void OnUpdateDeserializationResult(VLModelDeserializationResult[] modelsInformation)
    {
        this.modelDescriptions = new List<VLModelDeserializationResult>(modelsInformation);
    }
    
    /// <summary>
    ///  Dispatches the callback after models have been added, updated or 
    ///  removed.
    /// </summary>
    [MonoPInvokeCallback(typeof(VLWorker.JsonStringAndBinaryCallback))]
    private static void DispatchAddModelCallback(
        string description, 
        string data,
        System.UInt32 dataSize, 
        IntPtr clientData)
    {
        if (description == null)
        {
            return;
        }

        try
        {
            VLModelDeserializationStructure answerStructure =
                VLJsonUtility.FromJson<VLModelDeserializationStructure>(description);

            VLModelTrackableBehaviour trackable = 
                (VLModelTrackableBehaviour)GCHandle.FromIntPtr(clientData).Target;

            // free data previously allocated/pinned
            trackable.FreeNextBinaryMemoryBlock();

            if (!String.IsNullOrEmpty(answerStructure.error))
            {
                Debug.Log("[vlUnitySDK] VLModelTrackableBehaviour.DispatchAddModelCallback: Error occurred while adding a model to the tracking system: "
                    + answerStructure.error);
            }

            if (answerStructure.result != null)
            {
                trackable.OnUpdateDeserializationResult(answerStructure.result);
            }


        }
        catch (Exception e) // Catch all exceptions, because this is a callback
                            // invoked from native code
        {
            Debug.LogError("[vlUnitySDK] " + e.GetType().Name + ": " +
                e.Message);
        }
    }
    private static VLWorker.JsonStringAndBinaryCallback 
        dispatchAddModelCallbackDelegate =
        new VLWorker.JsonStringAndBinaryCallback(DispatchAddModelCallback);

    /// <summary>
    ///  Enables the model in the tracking system. If occluder is true, it will
    ///  be used as an occlusion geometry.
    /// </summary>
    public void SetUseModelForTracking(bool enable)
    {
        if (enable == this.internalUseForTracking)
        {
            return;
        }

        this.useForTracking = enable;
        this.internalUseForTracking = enable;

        foreach (VLModelDeserializationResult description in modelDescriptions)
        {
            this.modelTrackerBehaviour.SetModelProperty(description.name, "enabled", enable);
        }
    }

    /// <summary>
    ///  Defines, if this mesh should be used as an occlusion geometry in the 
    ///  tracking system. It is only active, if useForTracking is true.
    /// </summary>
    public void SetUseAsOccluder(bool enable)
    {
        if (enable == this.internalOccluder)
        {
            return;
        }

        this.occluder = enable;
        this.internalOccluder = enable;

        foreach (VLModelDeserializationResult description in modelDescriptions)
        {
            this.modelTrackerBehaviour.SetModelProperty(description.name, "occluder", enable);
        }
    }

    /// <summary>
    ///  Add all (sub)meshes to the tracking system. If a sub mesh has its own
    ///  VLModelTrackableBehaviour, it will not be added, but this behaviour should 
    ///  manage the relevant submeshes.
    /// </summary>
    public void UpdateModel(bool success = true)
    {
        if (success && this.modelTrackerBehaviour.workerBehaviour)
        {
            VLWorker worker = this.modelTrackerBehaviour.workerBehaviour.GetWorker();
            if (worker == null)
            {
                Debug.Log("[vlUnitySDK] VLModelTrackableBehaviour.UpdateModel: Worker is not correctly initialized\n");
                return;
            }

            AddModelDataCommandDescription command = 
                this.GenerateModelDataDescriptor(true);
            byte[] binaryData = this.GenerateBinaryData(
                this.modelData, this.binaryOffset);

            GCHandle binaryDataHandle =
                GCHandle.Alloc(binaryData, GCHandleType.Pinned);
            IntPtr data = binaryDataHandle.AddrOfPinnedObject();
            UInt32 dataLength = Convert.ToUInt32(binaryData.Length);
            gcHandleQueue.Enqueue(binaryDataHandle);

            worker.PushJsonAndBinaryCommand(
                VLJsonUtility.ToJson(command), 
                data, 
                dataLength,
                dispatchAddModelCallbackDelegate,
                GCHandle.ToIntPtr(this.gcHandle));
        }
    }

    /// <summary>
    ///  Updates the transformation of all (sub)meshes in the tracking system.
    ///  It has to be called after each update in a transform which is relevant
    ///  for the location of a related mesh. 
    /// </summary>
    /// <param name="useAllChildNodes">
    ///  If useAllChildNodes is true, this will update all locations of 
    ///  submeshes, even if they have their own VLModelTrackableBehaviour. It does 
    ///  not update the modelDescriptions of this behaviour.
    /// </param>
    public void UpdateTransformation(bool useAllChildNodes)
    {
        if (this.modelTrackerBehaviour.HasWorkerReference())
        {
            VLWorker worker = this.modelTrackerBehaviour.workerBehaviour.GetWorker();
            if (worker == null)
            {
                Debug.Log("[vlUnitySDK] VLModelTrackableBehaviour: Worker is not correctly initialized\n");
                return;
            }

            AddModelDataCommandDescription command = 
                this.GenerateModelDataDescriptor(false, useAllChildNodes);
            
            worker.PushJsonAndBinaryCommand(
                VLJsonUtility.ToJson(command),
                IntPtr.Zero,
                0,
                null,
                IntPtr.Zero);
        }
    }

    /// <summary>
    /// Generates an AddModelDataCommandDescription from all meshes, which 
    /// will be administered by this behaviour.
    /// </summary>
    /// <param name="addDataDescriptor"></param>
    /// Fills the ModelDataDescriptors with a DataDescriptor, a description of the 
    /// data structure of the model data. This will create more trafic and 
    /// completely delete and remove the model from memory. If you only have
    /// minor changes (transformation, model (de)activation, etc.) set this to 
    /// false.
    /// <param name="useAllChildNodes">
    /// If true: Also process child nodes, which are administered by another 
    /// VLModelTrackableBehaviour.
    /// </param>
    /// <returns></returns>
    private AddModelDataCommandDescription GenerateModelDataDescriptor(
        bool addDataDescriptor, bool useAllChildNodes = false)
    {
        // Reset global data
        this.binaryOffset = 0;
        this.modelData.Clear();
        this.globalUpdateCount++;

        AddModelDataCommandDescription commandDescriptor = 
            new AddModelDataCommandDescription();
        IterateThroughChildNodes(
            this.transform, 
            useAllChildNodes, 
            addDataDescriptor, 
            ref commandDescriptor);
        return commandDescriptor;
    }

    /// <summary>
    /// Iterates through all Child nodes and adds the data to the 
    /// commandDescriptor.
    /// </summary>
    /// <param name="transform">
    /// Transform, which is searched for possible meshes.
    /// </param>
    /// <param name="useAllChildNodes">
    /// If true: Also process child nodes, which are administered by another 
    /// VLModelTrackableBehaviour
    /// </param>
    /// <param name="addDataDescriptor">
    /// Fills the ModelDataDescriptors with a DataDescriptor, a description of the
    /// data structure of the model data.
    /// </param>
    /// <param name="commandDescriptor">
    /// Reference to the command structure of the json command, which will be 
    /// filled by this function. 
    /// </param>
    private void IterateThroughChildNodes(
        Transform transform, 
        bool useAllChildNodes,
        bool addDataDescriptor,
        ref AddModelDataCommandDescription commandDescriptor)
    {
        MeshFilter mesh = transform.GetComponent<MeshFilter>();

        // If child node has a mesh, add this to the list
        if (mesh && !this.AddModelDescription(
                transform,
                useAllChildNodes,
                addDataDescriptor, 
                ref commandDescriptor))
        {
            // If node is inactive or should not be added to commandDescriptor
            // do not iterate through the child nodes.
            return;
        }
        
        // Add child nodes
        foreach (Transform child in transform)
        {
            this.IterateThroughChildNodes(
                child, 
                useAllChildNodes, 
                addDataDescriptor, 
                ref commandDescriptor);
        }
    }

    /// <summary>
    /// Add a ModelDataDescriptors of the mesh inside the transform to the 
    /// commandDescriptor
    /// </summary>
    /// <param name="transform">
    /// Transform, which is searched for possible meshes.
    /// </param>
    /// <param name="useAllChildNodes">
    /// If true: Also process child nodes, which are administered by another 
    /// VLModelTrackableBehaviour
    /// </param>
    /// <param name="addDataDescriptor">
    /// Fills the ModelDataDescriptor with a DataDescriptor, a description of the
    /// data structure of the model data.
    /// </param>
    /// <param name="commandDescriptor">
    /// Reference to the command structure of the json command, which will be 
    /// filled by this function. 
    /// </param>
    /// <returns><c>True</c> if the model could be serialized into the visionlib.</c>False</c> if the data could not be gathered.</returns>
    private bool AddModelDescription(
        Transform transform, 
        bool useAllChildNodes, 
        bool addDataDescription,
        ref AddModelDataCommandDescription commandDescriptor)
    {
        // If transform is not active, do not add the model
        if (!transform.gameObject.activeInHierarchy)
        {
            return false;
        }

        // See if another VLModelTrackableBehaviour is active in this transform. If
        // this is the case, break execution of this node and its children.
        VLModelTrackableBehaviour trackable =
            transform.GetComponent<VLModelTrackableBehaviour>();
        if (!useAllChildNodes &&
            trackable &&
            trackable != this &&
            trackable.enabled)
        {
            return false;
        }
        if (trackable == null)
        {
            trackable = this;
        }


        Quaternion rotation = transform.rotation;
        Vector3 globalScale = GetGlobalScale(transform);
        Vector3 position = transform.position;

        // On HoloLens, the content node is added to the camera and thus the 
        // transformation of the mesh will be changed. This change has to be 
        // removed when streaming the data into the vlSDK
        Transform contentTransform = getContentTransform();
        if (contentTransform != null)
        {
            Vector3 contentGlobalScale = GetGlobalScale(contentTransform);
            rotation =
                Quaternion.Inverse(contentTransform.rotation) * rotation;
            globalScale = new Vector3(
                globalScale.x / contentGlobalScale.x,
                globalScale.y / contentGlobalScale.y,
                globalScale.z / contentGlobalScale.z);
            position =
                Quaternion.Inverse(contentTransform.rotation) *
                (position - contentTransform.position);
        }

        VLUnityCameraHelper.ToVLInPlace(ref position, ref rotation, modelTrackerBehaviour.workerBehaviour.flipCoordinateSystemHandedness);

        MeshFilter mesh = transform.GetComponent<MeshFilter>();
        string uniqueUnityModelID = mesh.GetInstanceID().ToString();

        ModelTransform modelTransform = new ModelTransform();
        modelTransform.t = new float[] { position.x, position.y, position.z};
        modelTransform.s = new float[] 
            { globalScale.x, globalScale.y, globalScale.z };
        modelTransform.q = new float[] 
            { rotation.x, rotation.y, rotation.z, rotation.w };

        ModelDataDescriptor descriptor = new ModelDataDescriptor();
        descriptor.name = uniqueUnityModelID;
        descriptor.type = "model";
        descriptor.enabled = trackable.useForTracking; 
        descriptor.occluder = trackable.occluder; 
        descriptor.transform = modelTransform;
        if (addDataDescription)
        {
            descriptor.subModels = new BinaryDataDescriptor[] 
                { CreateDataDescriptor(mesh) };
        }

        commandDescriptor.models.Add(descriptor);

        return true;
    }

    /// <summary>
    /// Finds the "content" node of the HoloLensTrackerBehaviour. Under 
    /// HoloLens this is relevant for calculating the correct (relative) 
    /// transform for Models which should be added to the vlSDK.
    /// </summary>
    /// <returns></returns>
    private Transform getContentTransform()
    {
        // VLHoloLensTrackerBehaviour specified explicitly?
        if (this.holoLensTrackerBehaviour != null)
        {
            return this.holoLensTrackerBehaviour.content.transform;
        }

        // Look for it at the same GameObject first
        this.holoLensTrackerBehaviour =
            GetComponent<VLHoloLensTrackerBehaviour>();
        if (this.holoLensTrackerBehaviour != null)
        {
            return this.holoLensTrackerBehaviour.content.transform;
        }

        // Try to find it anywhere in the scene
        this.holoLensTrackerBehaviour = FindObjectOfType<VLHoloLensTrackerBehaviour>();
        if (this.holoLensTrackerBehaviour != null)
        {
            return this.holoLensTrackerBehaviour.content.transform; ;
        }
        return null;
    }

    /// <summary>
    /// Creates a description of the mesh data of this meshfilter. Internally
    /// updates the binaryOffset and adds the ModelData to the modelData list.
    /// </summary>
    /// <param name="filter">
    /// MeshFilter of the mesh, which will be processed. 
    /// </param>
    /// <returns></returns>
    private BinaryDataDescriptor CreateDataDescriptor(MeshFilter filter)
    {
        Mesh mesh = filter.mesh;

        BinaryDataDescriptor descriptor = new BinaryDataDescriptor();
        descriptor.name = mesh.name;
        descriptor.binaryOffset = this.binaryOffset;
        descriptor.updateCount = this.globalUpdateCount;
        descriptor.vertexCount = mesh.vertices.Length;
        descriptor.triangleIndexCount = mesh.triangles.Length;
        descriptor.normalCount = mesh.normals.Length;

        // Vertices
        this.binaryOffset += mesh.vertices.Length * 3 * sizeof(float);
        // Triangles
        this.binaryOffset += mesh.triangles.Length * sizeof(UInt32);
        // Normals
        this.binaryOffset += mesh.normals.Length * 3 * sizeof(float);

        ModelData data = new ModelData();
        data.mesh = mesh;
        data.transform = filter.transform.worldToLocalMatrix;

        this.modelData.Add(data);

        return descriptor;
    }

    /// <summary>
    /// Transforms the ModelData list to a byte array of length dataSize
    /// </summary>
    /// <param name="modelData">
    /// List of all the models, which should be added to the byte array.
    /// </param> 
    /// <param name="dataSize">
    /// Length of the generated byte array. This has to be the size which is 
    /// required for all the meshes given in modelData.
    /// </param>
    /// <returns></returns>
    private byte[] GenerateBinaryData(List<ModelData> modelData, int dataSize)
    {
        // Generate Data Buffer
        byte[] binaryData = new byte[dataSize];
        int binaryDataOffset = 0;
        foreach (ModelData data in modelData)
        {
            SerializeModel(data, ref binaryData, ref binaryDataOffset);
        }
        return binaryData;
    }

    /// <summary>
    /// Adds the data of the given model to the byte array. The internal binary
    /// structure of each model is
    ///  - vertices (3 floats per vertex; vertexCount vertices)
    ///  - indices (triangleIndexCount UInt32)
    ///  - normals (3 floats per normal; normalCount normals)
    /// </summary>
    /// <param name="data">
    /// ModelData of the model, which should be added to the byte array.
    /// </param>
    /// <param name="binaryData">
    /// Target byte array, in which the model will be serialized.
    /// </param>
    /// <param name="binaryDataOffset">
    /// Current index, where data can be written in the array without 
    /// overriding previously added data.
    /// </param>
    private void SerializeModel(
        ModelData data, ref byte[] binaryData, ref int binaryDataOffset)
    {
        Mesh mesh = data.mesh;

        /* Binary Structure
         * - vertexCount * 3 float: vertices
         * - triangleIndexCount UInt32: indices
         * - normalCount * float: normals
        */

        // Vertices
        foreach (Vector3 vertex in mesh.vertices)
        {
            // The flip of the x-axis is necessary for streaming the model data
            // into the vlSDK.
            float[] vector = { -vertex.x, vertex.y, -vertex.z };

            Buffer.BlockCopy(
                vector, 0, binaryData, binaryDataOffset, 3 * sizeof(float));
            binaryDataOffset += 3 * sizeof(float);
        }

        // Triangles
        Buffer.BlockCopy(
            mesh.triangles, 
            0, 
            binaryData, 
            binaryDataOffset, 
            mesh.triangles.Length * sizeof(UInt32));
        binaryDataOffset += mesh.triangles.Length * sizeof(UInt32);

        // Normals
        foreach (Vector3 normal in mesh.normals)
        {
            float[] vector = { normal.x, normal.y, normal.z };

            Buffer.BlockCopy(
                vector, 0, binaryData, binaryDataOffset, 3 * sizeof(float));
            binaryDataOffset += 3 * sizeof(float);
        }
    }


    private void Awake()
    {
        // Get a handle to the current object and make sure, that the object
        // doesn't get deleted by the garbage collector. We then use this
        // handle as client data for the native callbacks. This allows us to
        // retrieve the current address of the actual object during the
        // callback execution. GCHandleType.Pinned is not necessary, because we
        // are accessing the address only through the handle object, which gets
        // stored in a global handle table.
        this.gcHandle = GCHandle.Alloc(this);
    }

    private void FindModelTrackerBehaviour()
    {
        // VLModelTrackerBehaviour_v1 specified explicitly?
        if (this.modelTrackerBehaviour != null)
        {
            return;
        }

        // Look for it at the same GameObject first
        this.modelTrackerBehaviour =
            GetComponent<VLModelTrackerBehaviour_v1>();
        if (this.modelTrackerBehaviour != null)
        {
            return;
        }

        // Try to find it anywhere in the scene
        this.modelTrackerBehaviour = FindObjectOfType<VLModelTrackerBehaviour_v1>();
        if (this.modelTrackerBehaviour != null)
        {
            return;
        }
        Debug.LogError("[vlUnitySDK] VLModelTrackableBehaviour could not find any reference VLModelTrackerBehaviour_v1. Please add a VLModelTrackerBehaviour_v1 somewhere in your scene.");
    }

    private void Start()
    {
       this.FindModelTrackerBehaviour();
    }

    private void OnDestroy()
    {
        // Destroy memory allocated with awakening of the behaviour or allocated during runtime.
        // This function is called when shutting down or switching to another Scene

        this.gcHandle.Free();
        this.FreeBinaryMemory();
    }

    private void Update()
    {
        this.SetUseModelForTracking(this.useForTracking);
        this.SetUseAsOccluder(this.occluder);
    }

    private void OnTrackerStopped(bool success)
    {
        this.FreeBinaryMemory();
    }

    private void OnEnable()
    {
        this.FindModelTrackerBehaviour();

        VLWorkerBehaviour.OnTrackerStopped += OnTrackerStopped;
        VLWorkerBehaviour.OnTrackerInitialized += UpdateModel;
        if (this.modelTrackerBehaviour.HasWorkerReference() && this.modelTrackerBehaviour.workerBehaviour.GetTrackingRunning())
        {
            // Add all meshes to the scene
            this.UpdateModel();
        }
    }

    private void OnDisable()
    {

        if (this.modelTrackerBehaviour.workerBehaviour)
        {
            foreach (VLModelDeserializationResult description in this.modelDescriptions)
            {
               this.modelTrackerBehaviour.RemoveModel(description.name);
            }
        }
        this.modelDescriptions.Clear();
        VLWorkerBehaviour.OnTrackerInitialized -= UpdateModel;
        VLWorkerBehaviour.OnTrackerStopped -= OnTrackerStopped;

    }


    /// <summary>
    /// Multiplies all scales of all hierarchy levels
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    private static Vector3 GetGlobalScale(Transform transform)
    {
        if (transform.parent)
        {
            Vector3 combinedScale = GetGlobalScale(transform.parent);
            combinedScale.x *= transform.localScale.x;
            combinedScale.y *= transform.localScale.y;
            combinedScale.z *= transform.localScale.z;
            return combinedScale;
        }

        return transform.localScale;
    }


}

/**@}*/
