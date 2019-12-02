/** @addtogroup vlUnitySDK
 *  @{
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
///  The VLWorkSpaceManager collects the VLWorkSpace definitions from the hierarchy and transfers
///  them to VisionLib during runtime.
///  **THIS IS SUBJECT TO CHANGE** Please do not rely on this code in productive environments.
/// </summary>
/// <seealso cref="VLWorkSpace"/>
[AddComponentMenu("VisionLib/AutoInit/VL WorkSpace Manager")]
public class VLWorkSpaceManager : VLWorkerReferenceBehaviour
{
    [Tooltip("Start pose learning automatically as soon as the tracker initialized")]
    public bool autoStartLearning = false;
    [Tooltip("Show progress of pose learning")]
    public bool showProgressBar = false;
    [HideInInspector]
    public Slider progressBar;

    private bool updateProgressBar = false;
    private const float hideProgressBarOnMaxValueAfterSeconds = 2.5f;

    /// <summary>
    /// Resets the tracking (hard) and initializes the learning process for AutoInit.
    /// </summary>
    public void LearnWorkSpaceInitData()
    {
        if (this.InitWorkerReference())
        {
            this.workerBehaviour.ResetTrackingHard();
        }
        PushWorkSpaces();

        if (showProgressBar)
        {
            InitProgressBar();
        }
    }

    /// <summary>
    /// Collects all VLWorkSpaces in the scene and creates an 
    /// VLWorkSpace.Configuration, which can be used as a parameter for 
    /// VLModelTrackerCommands.SetWorkSpacesCmd.
    /// </summary>
    /// <returns>Serializable configuration of WorkSpaces</returns>
    private VIS.WorkSpace.Configuration GetWorkSpaceConfiguration()
    {
        bool useCameraRotation = true;
        List<VIS.WorkSpace.Definition> workSpacesList = new List<VIS.WorkSpace.Definition>();
        foreach (VLWorkSpace workSpace in FindObjectsOfType<VLWorkSpace>())
        {
            workSpacesList.Add(workSpace.GetWorkSpaceDefinition(useCameraRotation));
        }
        return new VIS.WorkSpace.Configuration(workSpacesList.ToArray());
    }

    /// <summary>
    /// Creates the VLWorkSpace.Configuration and writes it into the given file.
    /// </summary>
    /// <param name="fileName">Path of the file to write the data in.</param>
    /// <remarks>
    ///  <para>
    ///   It's possible to use vlSDK file schemes (e.g. local_storage_dir) here.
    ///  </para>
    /// </remarks>
    public void WriteWorkSpaceConfigurationToFile(string fileName)
    {
        GetWorkSpaceConfiguration().WriteToFile(fileName);
    }

    /// <summary>
    /// Pushes the VLWorkSpace.Configuration to the vlSDK.
    /// </summary>
    private void PushWorkSpaces()
    {
        if (!this.InitWorkerReference())
        {
            return;
        }

        this.worker.PushCommand(
            new VLModelTrackerCommands.SetWorkSpacesCmd(GetWorkSpaceConfiguration()),
            null,
            System.IntPtr.Zero);
    }

    /// <summary>
    /// Shows and initializes the progress bar (if present) to show the learning progress.
    /// </summary>
    private void InitProgressBar()
    {
        if (this.progressBar == null)
        {
            return;
        }

        this.progressBar.gameObject.SetActive(true);
        this.progressBar.value = 0.0f;
        this.progressBar.maxValue = 1.0f;
        this.progressBar.gameObject.GetComponentInChildren<Text>().text = "Preparing AutoInit";
        this.updateProgressBar = true;
    }

    /// <summary>
    /// Updates progress value of AutoInit.
    /// </summary>
    /// <param name="state">Current tracking state</param>
    void UpdateAutoInitSetupProgress(VLTrackingState state)
    {
        if (!this.updateProgressBar)
        {
            return;
        }

        float progress = 0.0f;
        foreach (VLTrackingState.TrackingObject obj in state.objects)
        {
            progress += obj._AutoInitSetupProgress;
        }
        this.progressBar.value = progress / state.objects.Length;

        if (this.progressBar.value > 0.98f)
        {
            this.updateProgressBar = false;
            this.progressBar.value = this.progressBar.maxValue;
            this.progressBar.gameObject.GetComponentInChildren<Text>().text = "Ready";
            StartCoroutine(WaitAndHideProgressBar());
        }
    }

    /// <summary>
    /// Assigns the AutoStartLearning method to the OnImage event
    /// of the VLWorkerBehaviour after TrackerInitialized successfully
    /// </summary>
    /// <param name="success"></param>
    private void AssignAutoStartLearning(bool success)
    {
        if (success)
        {
            VLWorkerBehaviour.OnImage += AutoStartLearningAfterImage;
        }
    }

    /// <summary>
    /// Automatically starts LearnWorkSpaceInitData()
    /// after the VLWorkerBehaviour got an image the first time
    /// </summary>
    /// <param name="success"></param>
    private void AutoStartLearningAfterImage(VLImageWrapper vlImage)
    {
        VLWorkerBehaviour.OnImage -= AutoStartLearningAfterImage;

        if (this.autoStartLearning)
        {
            LearnWorkSpaceInitData();
        }
    }

    private IEnumerator WaitAndHideProgressBar()
    {
        yield return new WaitForSecondsRealtime(hideProgressBarOnMaxValueAfterSeconds);
        this.progressBar.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        VLWorkerBehaviour.OnTrackerInitialized += AssignAutoStartLearning;
        VLWorkerBehaviour.OnTrackingStates += UpdateAutoInitSetupProgress;
    }

    void OnDisable()
    {
        VLWorkerBehaviour.OnTrackerInitialized -= AssignAutoStartLearning;
        VLWorkerBehaviour.OnTrackingStates -= UpdateAutoInitSetupProgress;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(VLWorkSpaceManager))]
public class VLWorkSpaceManagerEditor : Editor
{
    private VLWorkSpaceManager workSpaceManager;
    private bool showDebug;

    void Reset()
    {
        this.workSpaceManager = target as VLWorkSpaceManager;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        this.showDebug = this.serializedObject.FindProperty("showProgressBar").boolValue;

        EditorGUI.BeginChangeCheck();

        if (this.showDebug)
        {
            this.serializedObject.FindProperty("progressBar").objectReferenceValue =
                (Slider) EditorGUILayout.ObjectField(
                    new GUIContent(
                        "Progress Bar",
                        "Slider which is used to show the progress of the pose learning"),
                    this.serializedObject.FindProperty("progressBar").objectReferenceValue,
                    typeof(Slider),
                    true);
        }

        if (EditorGUI.EndChangeCheck())
        {
            this.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(this.workSpaceManager);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                this.workSpaceManager.gameObject.scene);
        }
    }
}
#endif

/**@}*/
