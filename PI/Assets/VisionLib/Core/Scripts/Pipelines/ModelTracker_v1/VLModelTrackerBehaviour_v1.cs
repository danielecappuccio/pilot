/** @addtogroup vlUnitySDK
 *  @{
 */

using UnityEngine;
using UnityEngine.Events;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using AOT;
using VLWorkerCommands;

/// <summary>
///  The VLModelTrackerBehaviour_v1 contains all functions, which are specific 
///  for the ModelTracker_v1.
/// </summary>
[AddComponentMenu("VisionLib/VL Model Tracker Behaviour V1")]
public class VLModelTrackerBehaviour_v1 : VLWorkerReferenceBehaviour
{
    private GCHandle gcHandle;
    
    private static VLModelTrackerBehaviour_v1 GetInstance(IntPtr clientData)
    {
        return (VLModelTrackerBehaviour_v1)
            GCHandle.FromIntPtr(clientData).Target;
    }

    [System.Serializable]
    public class OnSetModelURIEvent : UnityEvent<string> { }

    /// <summary>
    ///  Event which will be emitted before "SetModelURI" has been called.
    /// </summary>
    public OnSetModelURIEvent OnSetModelURI;

    [System.Serializable]
    public class OnSetModelURIFinishedEvent : UnityEvent<bool> { }

    /// <summary>
    ///  Event which will be emitted after "SetModelURI" has been finished
    /// </summary>
    public OnSetModelURIFinishedEvent OnSetModelURIFinished;

    [MonoPInvokeCallback(typeof(VLWorker.JsonStringCallback))]
    private static void DispatchSetModelURICallback(
        string errorJson, string resultJson, IntPtr clientData)
    {
        try
        {
            GetInstance(clientData).SetModelURIFinished(errorJson, resultJson);
        }
        catch (Exception e) // Catch all exceptions, because this is a callback
                            // invoked from native code
        {
            Debug.LogError("[vlUnitySDK] " + e.GetType().Name + ": " +
                e.Message);
        }
    }

    private void SetModelURIFinished(string errorJson, string resultJson)
    { 
        if (this.OnSetModelURIFinished != null)
        {
            this.OnSetModelURIFinished.Invoke(errorJson != null);
        }
    }

    /// <summary>
    ///  Sets the modelURI to a new value and thus loads a new model.
    /// </summary>
    /// <param name="modelURI">
    ///  URI of the model, which should be used for tracking. 
    /// </param>
    public void SetModelURI(string modelURI)
    {
        if (this.InitWorkerReference())
        {
            if (this.OnSetModelURI != null)
            {
                this.OnSetModelURI.Invoke(modelURI);
            }

            this.worker.PushCommand(
                new VLWorkerCommands.SetAttributeCmd(
                    new VLWorkerCommands.SetAttributeCmd.Param(
                        "modelURI", modelURI)),
                DispatchSetModelURICallback,
                GCHandle.ToIntPtr(this.gcHandle));
        }
    }

    /// <summary>
    ///  Reset the tracking and all keyframes.
    /// </summary>
    public void ResetTrackingHard()
    {
        if (this.InitWorkerReference())
        {
            this.workerBehaviour.ResetTrackingHard();
        }
    }

    /// <summary>
    ///  Reset the tracking.
    /// </summary>
    public void ResetTrackingSoft()
    {
        if (this.InitWorkerReference())
        {
            this.workerBehaviour.ResetTrackingSoft();
        }
    }

    /// <summary>
    ///  Delegate for <see cref="WriteInitDataAction"/> events.
    /// </summary>
    /// <param name="success">
    ///  <c>true</c>, on success; <c>false</c>, otherwise.
    /// </param>
    public delegate void WriteInitDataAction(bool success);
    /// <summary>
    ///  Event which will be emitted after a the initialization were written
    ///  to disk.
    /// </summary>
    public static event WriteInitDataAction OnWriteInitData;

    /// <summary>
    ///  Delegate for <see cref="ReadInitDataAction"/> events.
    /// </summary>
    /// <param name="success">
    ///  <c>true</c>, on success; <c>false</c>, otherwise.
    /// </param>
    public delegate void ReadInitDataAction(bool success);
    /// <summary>
    ///  Event which will be emitted after a the initialization data has been loaded
    ///  from an uri.
    /// </summary>
    public static event ReadInitDataAction OnReadInitData;

    /// <summary>
    ///  Delegate for <see cref="ResetInitDataAction"/> events.
    /// </summary>
    /// <param name="success">
    ///  <c>true</c>, on success; <c>false</c>, otherwise.
    /// </param>
    public delegate void ResetInitDataAction(bool success);
    /// <summary>
    ///  Event which will be emitted after a the initialization data has been reset
    ///  from an uri.
    /// </summary>
    public static event ResetInitDataAction OnResetInitData;

    [MonoPInvokeCallback(typeof(VLWorker.JsonStringCallback))]
    private static void DispatchWriteInitDataCallback(
        string errorJson, string resultJson, IntPtr clientData)
    {
        try
        {
            GetInstance(clientData).WriteInitDataHandler(
                errorJson, resultJson);
        }
        catch (Exception e) // Catch all exceptions, because this is a callback
                            // invoked from native code
        {
            Debug.LogError("[vlUnitySDK] " + e.GetType().Name + ": " +
                e.Message);
        }
    }
    private static VLWorker.JsonStringCallback dispatchWriteInitDataCallbackDelegate =
        new VLWorker.JsonStringCallback(DispatchWriteInitDataCallback);

    [MonoPInvokeCallback(typeof(VLWorker.JsonStringCallback))]
    private static void DispatchReadInitDataCallback(
        string errorJson, string resultJson, IntPtr clientData)
    {
        try
        {
            GetInstance(clientData).ReadInitDataHandler(
                errorJson, resultJson);
        }
        catch (Exception e) // Catch all exceptions, because this is a callback
                            // invoked from native code
        {
            Debug.LogError("[vlUnitySDK] " + e.GetType().Name + ": " +
                e.Message);
        }
    }
    private static VLWorker.JsonStringCallback dispatchReadInitDataCallbackDelegate =
        new VLWorker.JsonStringCallback(DispatchReadInitDataCallback);

    [MonoPInvokeCallback(typeof(VLWorker.JsonStringCallback))]
    private static void DispatchResetInitDataCallback(
        string errorJson, string resultJson, IntPtr clientData)
    {
        try
        {
            GetInstance(clientData).ResetInitDataHandler(
                errorJson, resultJson);
        }
        catch (Exception e) // Catch all exceptions, because this is a callback
                            // invoked from native code
        {
            Debug.LogError("[vlUnitySDK] " + e.GetType().Name + ": " +
                e.Message);
        }
    }
    private static VLWorker.JsonStringCallback dispatchResetInitDataCallbackDelegate =
        new VLWorker.JsonStringCallback(DispatchResetInitDataCallback);


    /// <summary>
    ///  Write the captured initialization data as file to default location
    ///  with default name.
    /// </summary>
    /// <remarks>
    ///  A default name will be used for the file ("InitData_timestamp.binz").
    ///  The file will be written to different locations depending on the
    ///  platform:
    ///  * Windows: Current users home directory
    ///  * MacOS: Current users document directory
    ///  * iOS / Android: The current applications document directory
    /// </remarks>
    public void WriteInitData()
    {
		if (this.InitWorkerReference())
		{
	        this.worker.PushCommand(
	            new VLModelTrackerCommands.WriteInitDataCmd(),
	            dispatchWriteInitDataCallbackDelegate,
	            GCHandle.ToIntPtr(this.gcHandle));
		}
    }

    /// <summary>
    ///  Write the captured initialization data as file to custom location
    ///  with custom name.
    /// </summary>
    /// <remarks>
    ///  In order to avoid having to use a different file path for each
    ///  platform, the "local_storage_dir" scheme can be used as file prefix.
    ///  This scheme points to different locations depending on the platform:
    ///  * Windows: Current users home directory
    ///  * MacOS: Current users document directory
    ///  * iOS / Android: The current applications document directory
    /// </remarks>
    /// <param name="filePrefix">
    ///  Will be used as filename and path. A time stamp and the file
    ///  extension will be appended automatically. A plausible value could be
    ///  for example "local_storage_dir:MyInitData_".
    /// </param>
    public void WriteInitData(string filePrefix)
    {
		if (this.InitWorkerReference())
		{
			this.worker.PushCommand(
				new VLModelTrackerCommands.WriteInitDataWithPrefixCmd(filePrefix),
				dispatchWriteInitDataCallbackDelegate,
				GCHandle.ToIntPtr(this.gcHandle));
		}
    }

    /// <summary>
    ///  Loads the captured initialization data as file from a custom location.
    /// </summary>
    /// <remarks>
    ///  In order to load init data at best use a static uri. A common way is for each
    ///  platform, is using  "local_storage_dir" scheme which can be used as file prefix.
    ///  This scheme points to different locations depending on the platform:
    ///  * Windows: Current users home directory
    ///  * MacOS: Current users document directory
    ///  * iOS / Android: The current applications document directory
    /// </remarks>
    /// <param name="uri">
    ///  Will be used as filename and path. A time stamp and the file
    ///  extension will be appended automatically. A plausible value could be
    ///  for example "local_storage_dir:MyInitData_".
    /// </param>
    public void ReadInitData(string uri)
    {
		if (this.InitWorkerReference())
		{
        	this.worker.PushCommand(
            	new VLModelTrackerCommands.ReadInitDataWithPrefixCmd(uri),
            	dispatchReadInitDataCallbackDelegate,
            	GCHandle.ToIntPtr(this.gcHandle));
		}
    }

    /// <summary>
    ///  Reset the offline initialization data.
    /// </summary>
    /// <remarks>
    ///  In order to reset the initialization data loaded at the beginning this fuction can be called.
    ///  The init data learned on the fly, will still be maintained and can be reset by issuing a hard reset.
    /// </remarks>
    public void ResetInitData()
    {
		if (this.InitWorkerReference())
		{
			this.worker.PushCommand(
				new VLModelTrackerCommands.ResetInitDataCmd(),
				dispatchResetInitDataCallbackDelegate,
				GCHandle.ToIntPtr(this.gcHandle));

		}
    }

    public bool HasWorkerReference()
    {
        return this.InitWorkerReference();
    }

    private void WriteInitDataHandler(string errorJson, string resultJson)
    {
        if (OnWriteInitData != null)
        {
            OnWriteInitData(errorJson == null);
        }
    }

    private void ReadInitDataHandler(string errorJson, string resultJson)
    {
        if (OnReadInitData != null)
        {
            OnReadInitData(errorJson == null);
        }
    }

    private void ResetInitDataHandler(string errorJson, string resultJson)
    {
        if (OnResetInitData != null)
        {
            OnResetInitData(errorJson == null);
        }
    }


    [MonoPInvokeCallback(typeof(VLWorker.JsonStringCallback))]
    private static void DispatchGetModelPropertiesCallback(
        string errorJson, string resultJson, IntPtr clientData)
    {
        try
        {
            GetInstance(clientData).GetModelPropertiesHandler(errorJson, resultJson);
        }
        catch (Exception e) // Catch all exceptions, because this is a callback
                            // invoked from native code
        {
            Debug.LogError("[vlUnitySDK] " + e.GetType().Name + ": " +
                e.Message);
        }
    }
    private static VLWorker.JsonStringCallback dispatchGetModelPropertiesCallbackDelegate =
        new VLWorker.JsonStringCallback(DispatchGetModelPropertiesCallback);

    public bool SetModelProperty(string name, string property, bool state)
    {
		if (this.InitWorkerReference())
		{
	        return this.worker.PushCommand(
	            new VLModelTrackerCommands.SetModelBoolPropertyCmd(name, property, state),
	            null,
	            IntPtr.Zero);
	    }
		return false;
	}

    public bool GetModelProperties()
    {
		if (this.InitWorkerReference())
		{
			return this.worker.PushCommand(
				new VLModelTrackerCommands.GetModelPropertiesCmd(),
				dispatchGetModelPropertiesCallbackDelegate,
				GCHandle.ToIntPtr(this.gcHandle));
		}
        return false;
    }

    private void GetModelPropertiesHandler(string errorJson, string resultJson)
    {
        if (OnGetModelProperties != null)
        {
            VLModelPropertiesStructure modelProperties = null;
            if (resultJson != null)
            {
                modelProperties =
                    VLJsonUtility.FromJson<VLModelPropertiesStructure>(resultJson);
                OnGetModelProperties(modelProperties.info);
            }
            else
            {
                OnGetModelProperties(null);
            }
        }
    }

    /// <summary>
    ///  Delegate for <see cref="OnGetModelProperties"/> events.
    /// </summary>
    /// <param name="properties">
    ///  <c>VLModelProperties</c> as an Array of actual Model Properties
    /// </param>
    public delegate void GetModelPropertiesAction(VLModelProperties[] properties);
    /// <summary>
    ///  Event which will be emitted when the model properties have been rquested by GetModelStateProperties. 
    /// </summary>
    public static event GetModelPropertiesAction OnGetModelProperties;


    public void RemoveModel(string name)
    {
        if (this.worker == null)
        {
            return;
        }

        this.worker.PushCommand(
            new VLModelTrackerCommands.RemoveModelCmd(name),
            null,
            IntPtr.Zero);
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

    private void OnDestroy()
    {
        // Release the handle to the current object
        this.gcHandle.Free();
    }

}

/**@}*/