/** @addtogroup vlUnitySDK
 *  @{
 */

using System;
using UnityEngine;
using VLHoloLensModelTrackerCommands;
using System.Runtime.InteropServices;

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
///  Manages the model-based initialization of the HoloLens tracking.
/// </summary>
/// <remarks>
///  Right now this behaviour only works correctly, if there also is an
///  enabled VLHoloLensInitCameraBehaviour somewhere in the scene.
/// </remarks>
[AddComponentMenu("VisionLib/VL HoloLens Tracker Behaviour")]
public class VLHoloLensTrackerBehaviour : VLWorkerReferenceBehaviour
{
    /// <summary>
    ///  The GameObject representing the HoloLens camera.
    /// </summary>
    ///  If this is not defined, then the VLHoloLensTrackerBehaviour tries to
    ///  find the camera automatically. It will first try to find a camera
    ///  component on the current GameObject. Then it will use the main camera.
    ///  If this also fails, it will use any camera available in the current
    ///  scene.
    /// </remarks>
    [Tooltip("The GameObject representing the HoloLens camera. If this is not defined, then it will get set automatically.")]
    public GameObject holoLensCamera;

    /// <summary>
    ///  GameObject with the AR content attached to it.
    /// </summary>
    /// <remarks>
    ///  Any existing transformation of the content GameObject will get
    ///  overwritten. If you need to transform the content, then please add
    ///  a child GameObject and apply the transformation to it instead.
    /// </remarks>
    [Tooltip("GameObject with the AR content attached to it")]
    public GameObject content;

    public float smoothTime = 1.0f / 10.0f;

#if (UNITY_EDITOR || UNITY_WSA_10_0)
    private GameObject worldAnchorGO;

    private float[] modelViewMatrixArray = new float[16];
    private Matrix4x4 modelViewMatrix = new Matrix4x4();

    private bool initMode = true;

    private Vector3 contentTargetPosition = Vector3.zero;
    private Vector3 contentTargetPositionVelocity = Vector3.zero;
    private Quaternion contentTargetRotation = Quaternion.identity;

    private int updateIgnoreCounter = 0; // TODO(mbuchner): Find a better
                                         // solution for ignoring the
                                         // OnExtrinsicData call

    private IntPtr globalCoordinateSystem = IntPtr.Zero;

    private void OnTrackerInitializing()
    {
        this.contentTargetPosition = Vector3.zero;
        this.contentTargetPositionVelocity = Vector3.zero;
        this.contentTargetRotation = Quaternion.identity;
        this.updateIgnoreCounter = 0;

        this.ActivateInitMode();
    }

    private void OnTrackerInitialized(bool success)
    {
        SetGlobalCoordinateSystemInVisionLib();
    }

    private void OnExtrinsicData(VLExtrinsicDataWrapper extrinsicData)
    {
        if (this.updateIgnoreCounter > 0)
        {
            this.updateIgnoreCounter -= 1;
            return;
        }

        // State changed from invalid to valid?
        bool valid = extrinsicData.GetValid();
        if (valid && this.initMode)
        {
            this.DeactivateInitMode();
        }
        // State changed from valid to invalid?
        else if (!valid && !this.initMode)
        {
            // Don't go to initialization mode, because the HoloLens is able to
            // relocate itself
            //this.ActivateInitMode();
        }

        if (!extrinsicData.GetModelViewMatrix(this.modelViewMatrixArray))
        {
            return;
        }

        if (valid)
        {
            // Apply the extrinsic camera parameters
            for (int i = 0; i < 16; ++i)
            {
                this.modelViewMatrix[i % 4, i / 4] =
                    this.modelViewMatrixArray[i];
            }

            if (this.workerBehaviour.flipCoordinateSystemHandedness == 
                VLUnityCameraHelper.FlipCoordinateSystemHandedness.Automatically)
            {
                this.modelViewMatrix *= VLUnityCameraHelper.rotationY180;
            }

            // Compute the left-handed world to camera matrix
            this.modelViewMatrix = VLUnityCameraHelper.flipZ *
                VLUnityCameraHelper.flipYZ *
                this.modelViewMatrix *
                VLUnityCameraHelper.flipZ;

            Vector3 t = this.modelViewMatrix.GetColumn(3);
            Quaternion q = Quaternion.LookRotation(
                this.modelViewMatrix.GetColumn(2),
                this.modelViewMatrix.GetColumn(1));

            // Don't set the position directly. Interpolate smoothly in the
            // Update function instead
            this.contentTargetPosition = t;
            this.contentTargetRotation = q;
        }
    }

    private void OnTrackerReset(bool success)
    {
        this.ActivateInitMode();
        this.updateIgnoreCounter = 1; // Ignore the next OnExtrinsicData call,
                                      // because it might contain the previous
                                      // (valid) tracking pose
    }

    public void ActivateInitMode()
    {
        if (this.initMode)
        {
            return;
        }

        this.initMode = true;

        // Attach content to HoloLens camera
        if (this.content != null && this.holoLensCamera != null)
        {
            this.content.transform.parent = this.holoLensCamera.transform;
        }
    }

    private void DeactivateInitMode()
    {
        if (!this.initMode)
        {
            return;
        }

        this.initMode = false;

        // Detach content from HoloLens camera and attach to world anchor
        // GameObject
        if (this.content != null)
        {
            this.content.transform.parent = this.worldAnchorGO.transform;
            // When changing the parent node, Unity will keep the same
            // position and rotation in the world. Therefore we don't have to
            // convert the initial pose to world coordinates by ourself.
        }

        this.contentTargetPositionVelocity = Vector3.zero;
    }

    private void SetGlobalCoordinateSystemInVisionLib()
    {
        if (this.globalCoordinateSystem != IntPtr.Zero)
        {
            Marshal.Release(this.globalCoordinateSystem);
        }

        this.globalCoordinateSystem = WorldManager.GetNativeISpatialCoordinateSystemPtr();

        if (this.globalCoordinateSystem == IntPtr.Zero)
        {
            Debug.LogError("[vlUnitySDK] Failed to retrieve spatial coordinate system");
            return;
        }

        if (!this.InitWorkerReference())
        {
            Debug.LogError("[vlUnitySDK] Failed to get the VLWorker from VLHoloLensinitCameraBehaviour for SetGlobalCoordinateSystemInVisionLib");
            return;
        }

        this.worker.PushJsonAndBinaryCommand(
            VLJsonUtility.ToJson(
                new VLHoloLensModelTrackerCommands.
                SetGlobalCoordinateSystemCommandDescription()),
            this.globalCoordinateSystem,
            0,
            null,
            IntPtr.Zero);
    }

    private bool InitCameraReference()
    {
        // HoloLens camera specified manually or previously found?
        if (this.holoLensCamera != null)
        {
            return true;
        }

        // Look for it at the same GameObject first
        Camera camera = GetComponent<Camera>();
        if (camera != null)
        {
            this.holoLensCamera = camera.gameObject;
            return true;
        }

        // Use the main camera
        camera = Camera.main;
        if (camera != null)
        {
            this.holoLensCamera = camera.gameObject;
            return true;
        }

        // Try to find it anywhere in the scene
        camera = FindObjectOfType<Camera>();
        if (camera != null)
        {
            this.holoLensCamera = camera.gameObject;
            return true;
        }

        return false;
    }

    private void Awake()
    {
        // Create the GameObject, which represents the origin of the HoloLens
        // coordinate system in Unity
        this.worldAnchorGO = new GameObject("VLHoloLensWorldAnchor");

        if (this.content == null)
        {
            Debug.LogWarning("[vlUnitySDK] Content is null. Did you forget to set the 'content' property?");
        }
    }

    private void OnEnable()
    {
        VLWorkerBehaviour.OnTrackerInitializing += OnTrackerInitializing;
        VLWorkerBehaviour.OnTrackerInitialized += OnTrackerInitialized;
        VLWorkerBehaviour.OnExtrinsicData += OnExtrinsicData;
        VLWorkerBehaviour.OnTrackerResetSoft += OnTrackerReset;
        VLWorkerBehaviour.OnTrackerResetHard += OnTrackerReset;
        this.worldAnchorGO.SetActive(true);
    }

    private void OnDisable()
    {
        // GameObject not destroyed already?
        if (this.worldAnchorGO != null)
        {
            this.worldAnchorGO.SetActive(false);
        }
        VLWorkerBehaviour.OnTrackerResetHard -= OnTrackerReset;
        VLWorkerBehaviour.OnTrackerResetSoft -= OnTrackerReset;
        VLWorkerBehaviour.OnExtrinsicData -= OnExtrinsicData;
        VLWorkerBehaviour.OnTrackerInitialized -= OnTrackerInitialized;
        VLWorkerBehaviour.OnTrackerInitializing -= OnTrackerInitializing;
    }

    private void Start()
    {
        if (!this.InitWorkerReference())
        {
            Debug.LogWarning("[vlUnitySDK] Failed to get the VLWorker from VLWorkerBehaviour");
        }

        if (!this.InitCameraReference())
        {
            Debug.LogWarning("[vlUnitySDK] Could not find HoloLens camera");
        }

        this.initMode = false;
        this.ActivateInitMode();
    }

    private void OnDestroy()
    {
        if (this.globalCoordinateSystem != IntPtr.Zero)
        {
            Marshal.Release(this.globalCoordinateSystem);
            this.globalCoordinateSystem = IntPtr.Zero;
        }
    }

    private void Update()
    {
        if (this.content == null)
        {
            return;
        }

        if (!this.initMode)
        {
            // Interpolate the transformation of the content
            if (this.smoothTime > 0)
            {
                this.content.transform.localPosition = Vector3.SmoothDamp(
                    this.content.transform.localPosition,
                    this.contentTargetPosition,
                    ref this.contentTargetPositionVelocity,
                    this.smoothTime);

                float elapsedTime = Mathf.Min(Time.deltaTime, this.smoothTime);
                this.content.transform.localRotation = Quaternion.Slerp(
                    this.content.transform.localRotation,
                    this.contentTargetRotation,
                    0.5f * (elapsedTime / this.smoothTime));
            }
            else
            {
                this.content.transform.localPosition = this.contentTargetPosition;
                this.content.transform.localRotation = this.contentTargetRotation;
            }
        }
    }
#else // Empty dummy implementation
    private void Awake()
    {
    }

    private void Start()
    {
        Debug.LogWarning("[vlUnitySDK] The VLHoloLensTrackerBehaviour only works for Windows Store applications");
    }

    private void Update()
    {
    }
#endif // (UNITY_EDITOR || UNITY_WSA_10_0)
}

/**@}*/
