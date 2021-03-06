﻿/** @addtogroup vlUnitySDK
 *  @{
 */

using AOT;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using VLHoloLensModelTrackerCommands;

#if (UNITY_EDITOR || UNITY_WSA_10_0)
#if UNITY_2017_2_OR_NEWER
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;
#else // UNITY_2017_2_OR_NEWER
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;
#endif // UNITY_2017_2_OR_NEWER
#endif // (UNITY_EDITOR || UNITY_WSA_10_0)

/// <summary>
///  Camera used to define the initial pose for the HoloLens model-based
///  tracking.
/// </summary>
/// <remarks>
///  <para>
///   If there is no VLHoloLensInitCameraBehaviour in the scene or the
///   VLHoloLensInitCameraBehaviour is disabled, then the HoloLens model-based
///   tracking will not work correctly.
///  </para>
///  <para>
///   It's possible to change the camera position and orientation at
///   runtime. The new initial pose will then be used while the tracking is
///   lost.
///  </para>
///  <para>
///   Please make sure, that there is only one active
///   VLHoloLensInitCameraBehaviour in the scene. Otherwise both behaviours
///   will try to set the initial pose, which will lead to unexpected
///   behaviour.
///  </para>
/// </remarks>
[AddComponentMenu("VisionLib/VL HoloLens Init Camera Behaviour")]
public class VLHoloLensInitCameraBehaviour : VLWorkerReferenceBehaviour
{
    /// <summary>
    ///  Reference to used VLHoloLensTrackerBehaviour.
    /// </summary>
    /// <remarks>
    ///  If this is not defined, then the first found
    ///  VLHoloLensTrackerBehaviour will be used automatically.
    /// </remarks>
    public VLHoloLensTrackerBehaviour holoLensTrackerBehaviour;

    /// <summary>
    ///  Reference to the Camera behaviour.
    /// </summary>
    /// <remarks>
    ///  If this is not defined, then the Camera component attached to the
    ///  current GameObject will be used automatically.
    /// </remarks>
    public Camera initCamera;

    /// <summary>
    ///  Overwrite camera transformation with values from tracking
    ///  configuration.
    /// </summary>
    /// <remarks>
    ///  The InitCamera can then be transformed afterwards, but will get
    ///  overwritten again after loading a new tracking configuration.
    /// </remarks>
    [Tooltip("Overwrite camera transformation with values from tracking configuration")]
    public bool overwriteOnLoad;

    /// <summary>
    ///  Adapt initial pose to always be upright.
    /// </summary>
    /// <remarks>
    ///  This only works correctly, if <see cref="upAxis"/> actually is the
    ///  up-axis of the content.
    /// </remarks>
    [Tooltip("Adapt initial pose to always be upright")]
    public bool keepUpright = false;

    /// <summary>
    ///  Defines the up-axis used by the <see cref="keepUpright"/> option.
    /// </summary>
    /// <remarks>
    ///  In Unity usually the y-axis points up.
    /// </remarks>
    [Tooltip("Defines the up-axis used by the keepUpright option (usually the y-axis points up)")]
    public Vector3 upAxis = Vector3.up;

#if (UNITY_EDITOR || UNITY_WSA_10_0)
    private VLRenderRotation renderRotation = VLRenderRotation.CCW0;

    private float[] projectionMatrixArray = new float[16];
    private Matrix4x4 projectionMatrix = new Matrix4x4();

    private GCHandle gcHandle;
    private bool initPoseReady;
    private bool initMode = true;
    private bool reset;

    private Vector3 originalPosition;
    private Quaternion originalOrientation;

    private int updateIgnoreCounter = 0; // TODO(mbuchner): Find a better
                                         // solution for ignoring the
                                         // OnExtrinsicData call

    private const int maxSetGlobalObjectPoseCommands = 5;
    private int setGlobalObjectPoseCounter = 0;

    /// <summary>
    ///  Restores the original transformation of the InitCamera.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This might be useful if the InitCamera was transformed in some
    ///   awkward way for some reason and we quickly want to restore the
    ///   original state.
    ///  </para>
    ///  <para>
    ///   If <see cref="overwriteOnLoad"/> is set to <c>false</c>, then this
    ///   will restore the transformation during the initialization of the
    ///   VLHoloLensInitCameraBehaviour. If <see cref="overwriteOnLoad"/> is
    ///   set to <c>true</c>, then this will restore the transformation from
    ///   the tracking configuration.
    ///  </para>
    /// </remarks>
    public void Reset()
    {
        this.reset = true;
    }

    private bool IsReady()
    {
        return this.initPoseReady &&
            this.initCamera != null;
    }

    private static VLHoloLensInitCameraBehaviour GetInstance(IntPtr clientData)
    {
        return (VLHoloLensInitCameraBehaviour)GCHandle.FromIntPtr(clientData).Target;
    }

    [MonoPInvokeCallback(typeof(VLWorker.JsonStringCallback))]
    private static void DispatchSetGlobalObjectPoseCallback(string errorJson,
        string resultJson, IntPtr clientData)
    {
        try
        {
            GetInstance(clientData).ReduceSetGlobalObjectPoseCounter();
        }
        catch (Exception e) // Catch all exceptions, because this is a callback
                            // invoked from native code
        {
            Debug.LogError("[vlUnitySDK] " + e.GetType().Name + ": " +
                e.Message);
        }
    }
    private static VLWorker.JsonStringCallback dispatchSetGlobalObjectPoseCallbackDelegate =
        new VLWorker.JsonStringCallback(DispatchSetGlobalObjectPoseCallback);

    [MonoPInvokeCallback(typeof(VLWorker.JsonStringCallback))]
    private static void DispatchGetInitPoseCallback(string errorJson,
        string resultJson, IntPtr clientData)
    {
        try
        {
            GetInstance(clientData).OnGetInitPoseCallback(
                errorJson, resultJson);
        }
        catch (Exception e) // Catch all exceptions, because this is a callback
                            // invoked from native code
        {
            Debug.LogError("[vlUnitySDK] " + e.GetType().Name + ": " +
                e.Message);
        }
    }
    private static VLWorker.JsonStringCallback dispatchGetInitPoseCallbackDelegate =
        new VLWorker.JsonStringCallback(DispatchGetInitPoseCallback);

    private void ReduceSetGlobalObjectPoseCounter()
    {
        this.setGlobalObjectPoseCounter -= 1;
    }

    private void OnTrackerInitializing()
    {
        this.initPoseReady = false;
        this.reset = false;
        this.initMode = true;
        this.updateIgnoreCounter = 0;
        this.setGlobalObjectPoseCounter = 0;
    }

    private void OnTrackerInitialized(bool success)
    {
        if (!this.overwriteOnLoad)
        {
            this.initPoseReady = true;
        }
        else
        {
            this.GetInitPose();
            // We wait for the intial pose. initPoseReady will then get set
            // inside the callback from GetInitPose.
        }
    }

    private void OnExtrinsicData(VLExtrinsicDataWrapper extrinsicData)
    {
        if (this.updateIgnoreCounter > 0)
        {
            this.updateIgnoreCounter -= 1;
            return;
        }

        bool valid = extrinsicData.GetValid();

        // State changed from invalid to valid?
        if (valid && this.initMode)
        {
            this.initMode = false;
        }
        // State changed from valid to invalid?
        else if (!valid && !this.initMode)
        {
            // Don't go to initialization mode, because the HoloLens is able to
            // relocate itself
            //this.initMode = true;
        }
    }

    private void OnIntrinsicData(VLIntrinsicDataWrapper intrinsicData)
    {
        if (this.initCamera == null)
        {
            return;
        }

        // Apply the intrinsic camera parameters
        if (intrinsicData.GetProjectionMatrix(
            this.initCamera.nearClipPlane,
            this.initCamera.farClipPlane,
            Screen.width, Screen.height, this.renderRotation, 0,
            this.projectionMatrixArray))
        {
            for (int i = 0; i < 16; ++i)
            {
                this.projectionMatrix[i % 4, i / 4] =
                    this.projectionMatrixArray[i];
            }
            this.initCamera.projectionMatrix = this.projectionMatrix;
        }
    }

    private void OnTrackerReset(bool success)
    {
        this.initMode = true;
        this.updateIgnoreCounter = 1; // Ignore the next OnExtrinsicData call,
                                      // because it might contain the previous
                                      // (valid) tracking pose
    }

    private void OnGetInitPoseCallback(string errorJson, string resultJson)
    {
        if (errorJson != null)
        {
            Debug.LogWarning("[vlUnitySDK] OnGetInitPoseCallback: Failed to get init pose");
            return;
        }

        VLModelTrackerCommands.GetInitPoseResult result =
            VLJsonUtility.FromJson<VLModelTrackerCommands.GetInitPoseResult>(
                resultJson);

        Vector3 position;
        Quaternion orientation;
        VLUnityCameraHelper.VLPoseToCamera(
            new Vector3(result.t[0], result.t[1], result.t[2]),
            new Quaternion(result.q[0], result.q[1], result.q[2], result.q[3]),
            out position, out orientation,
            this.workerBehaviour.flipCoordinateSystemHandedness);

        this.originalPosition = position;
        this.originalOrientation = orientation;
        this.initPoseReady = true;
        this.reset = true; // This will set the new pose during the next Update
                           // call
    }

    private void GetInitPose()
    {
        if (this.InitWorkerReference())
        {
            this.worker.PushCommand(
                new VLModelTrackerCommands.GetInitPoseCmd(),
                dispatchGetInitPoseCallbackDelegate,
                GCHandle.ToIntPtr(this.gcHandle));
        }
        else
        {
            Debug.LogWarning("[vlUnitySDK] GetInitPose: Failed to get the VLWorker from VLWorkerBehaviour");
        }
    }

    private void SetInitPose()
    {
        if (!IsReady())
        {
            Debug.LogWarning("[vlUnitySDK] SetInitPose called while not ready");
            return;
        }

        GameObject content = (this.holoLensTrackerBehaviour != null ?
            this.holoLensTrackerBehaviour.content : null);
        if (content == null)
        {
            Debug.LogWarning("[vlUnitySDK] SetInitPose: No VLHoloLensTrackerBehaviour in the scene or its content is not specified");
            return;
        }

        if (!this.InitWorkerReference() || !workerBehaviour.GetTrackingRunning())
        {
            return;
        }

        // To prevent the vlSDK from getting more `SetGlobalObjectPose` calls 
        // than can be processed in time, we limit the amount of 
        // `SetGlobalObjectPose` commands.
        if (this.setGlobalObjectPoseCounter >= maxSetGlobalObjectPoseCommands)
        {
            return;
        }
        this.setGlobalObjectPoseCounter += 1;

        // Turn the camera pose into a content transformation
        Matrix4x4 worldToInitCameraMatrix =
            this.initCamera.transform.worldToLocalMatrix;
        Vector3 initContentLocalPosition =
            worldToInitCameraMatrix.GetColumn(3);
        Quaternion initContentLocalOrientation =
            VLUnityCameraHelper.QuaternionFromMatrix(
                worldToInitCameraMatrix);

        content.transform.localPosition =
            initContentLocalPosition;
        content.transform.localRotation =
            initContentLocalOrientation;
        if (this.keepUpright)
        {
            Vector3 contentUp = content.transform.rotation * this.upAxis;
            Quaternion upRotation = Quaternion.FromToRotation(
                contentUp, Vector3.up);
            content.transform.rotation =
                upRotation * content.transform.rotation;
        }

        Matrix4x4 globalObjectMatrix = content.transform.localToWorldMatrix;
        if (this.workerBehaviour.flipCoordinateSystemHandedness == 
            VLUnityCameraHelper.FlipCoordinateSystemHandedness.Automatically)
        {
            globalObjectMatrix *= VLUnityCameraHelper.rotationY180;
        }

        // Compute the right-handed global object transformation
        globalObjectMatrix = VLUnityCameraHelper.flipZ *
            globalObjectMatrix *
            VLUnityCameraHelper.flipZ;

        // We need to provide the global coordinate system once and push the 
        // current position of the content to the tracker in every frame.
        Vector3 t = globalObjectMatrix.GetColumn(3);
        Quaternion q = Quaternion.LookRotation(
            globalObjectMatrix.GetColumn(2),
            globalObjectMatrix.GetColumn(1));

        VLHoloLensModelTrackerCommands.SetGlobalObjectPoseCmd.Param param =
            new VLHoloLensModelTrackerCommands.SetGlobalObjectPoseCmd.Param(
                t.x,
                t.y,
                t.z,
                q.x,
                q.y,
                q.z,
                q.w);

        this.worker.PushCommand(
            new VLHoloLensModelTrackerCommands.SetGlobalObjectPoseCmd(param),
            dispatchSetGlobalObjectPoseCallbackDelegate,
            GCHandle.ToIntPtr(this.gcHandle));
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

        // Get the initCamera from the current GameObject, if it wasn't
        // specified explicitly
        if (this.initCamera == null)
        {
            this.initCamera = this.GetComponent<Camera>();
        }

        if (this.initCamera != null)
        {
            // Store the original transformation so we can restore it later
            this.originalPosition = this.initCamera.transform.position;
            this.originalOrientation = this.initCamera.transform.rotation;
        }
        else
        {
            Debug.LogWarning("[vlUnitySDK] initCamera not found. Add Camera behaviour to GameObject or set reference manually.");
        }
    }

    private void Start()
    {
        // Find the first VLHoloLensTrackerBeavhiour, if it wasn't specified
        // explicitly
        if (this.holoLensTrackerBehaviour == null)
        {
            // Automatically find VLHoloLensTrackerBehaviour
            this.holoLensTrackerBehaviour =
                FindObjectOfType<VLHoloLensTrackerBehaviour>();
            if (this.holoLensTrackerBehaviour == null)
            {
                Debug.LogWarning("[vlUnitySDK] No GameObject with VLHoloLensTrackerBehaviour found");
            }
        }
    }

    private void OnDestroy()
    {
        this.gcHandle.Free();
    }

    private void OnEnable()
    {
        VLWorkerBehaviour.OnExtrinsicData += OnExtrinsicData;
        VLWorkerBehaviour.OnIntrinsicData += OnIntrinsicData;
        VLWorkerBehaviour.OnTrackerInitializing += OnTrackerInitializing;
        VLWorkerBehaviour.OnTrackerInitialized += OnTrackerInitialized;
        VLWorkerBehaviour.OnTrackerResetSoft += OnTrackerReset;
        VLWorkerBehaviour.OnTrackerResetHard += OnTrackerReset;
    }

    private void OnDisable()
    {
        VLWorkerBehaviour.OnTrackerResetHard -= OnTrackerReset;
        VLWorkerBehaviour.OnTrackerResetSoft -= OnTrackerReset;
        VLWorkerBehaviour.OnTrackerInitialized -= OnTrackerInitialized;
        VLWorkerBehaviour.OnTrackerInitializing -= OnTrackerInitializing;
        VLWorkerBehaviour.OnIntrinsicData -= OnIntrinsicData;
        VLWorkerBehaviour.OnExtrinsicData -= OnExtrinsicData;
    }

    private void Update()
    {
        if (IsReady())
        {
            if (this.reset)
            {
                this.initCamera.transform.position = this.originalPosition;
                this.initCamera.transform.rotation = this.originalOrientation;
                this.reset = false;
            }

            if (this.initMode)
            {
                // This must be called continuously. Otherwise the content will
                // appear anchored somewhere in the world and not positioned
                // in front of the camera
                this.SetInitPose();
            }
        }
    }
#else // Empty dummy implementation
    private void Awake()
    {
    }

    private void Start()
    {
        Debug.LogWarning("[vlUnitySDK] The VLHoloLensInitCameraBehaviour only works for Windows Store applications");
    }

    private void Update()
    {
    }
#endif // (UNITY_EDITOR || UNITY_WSA_10_0)
}

/**@}*/
