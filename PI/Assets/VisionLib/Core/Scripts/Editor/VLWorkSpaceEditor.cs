using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor script modifying and displaying relevant VLWorkSpace values.
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(VLWorkSpace))]
public class VLWorkSpaceEditor : Editor
{
    private VLWorkSpace workSpace;

    private const int maxLinesToDraw = 600;

    private void Reset()
    {
        this.workSpace = target as VLWorkSpace;
        UpdatePoses();
        ApplySerializedValues();
    }

    /// <summary>
    /// Update the poses for the WorkSpace by taking the poses from vlSDK.
    /// Reset camSliderPosition if it's greater than the number of poses.
    /// </summary>
    private void UpdatePoses()
    {
        if (this.workSpace.destinationGeometry == null || this.workSpace.sourceGeometry == null)
        {
            this.workSpace.camSliderPosition = 0;
            this.workSpace.poses = new VIS.WorkSpace.Transform[0];
            return;
        }

        if (this.workSpace.sourceGeometry.currentMesh == null)
        {
            this.workSpace.sourceGeometry.UpdateMesh();
        }

        bool useCameraRotation = false;
        this.workSpace.poses = this.workSpace.GetWorkSpaceDefinition(useCameraRotation).GetCameraTransforms();

        if (this.workSpace.camSliderPosition > this.workSpace.poses.Length)
        {
            this.workSpace.camSliderPosition = 0;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!this.workSpace)
        {
            Reset();
        }

        DisplayInspectorParamaters();
    }

    private void DisplayInspectorParamaters()
    {
        EditorGUI.BeginChangeCheck();

        DisplayUpVector();
        DisplaySourceAndDestinationGeometry();
        DisplayViewDirections();
        DisplayPreviewInCamera();
        DisplayHelpBox();

        // apply if any parameter has been changed
        if (EditorGUI.EndChangeCheck())
        {
            ApplySerializedValues();
            EditorUtility.SetDirty(this.workSpace);
            if (!EditorApplication.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(this.workSpace.gameObject.scene);
            }
        }
    }

    private void DisplayUpVector()
    {
        this.workSpace.upVector = (Vector3) EditorGUILayout.Vector3Field(
            new GUIContent("Up Vector", "The up-Vector of your 3D object"),
            this.workSpace.upVector);
    }

    private void DisplaySourceAndDestinationGeometry()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUI.BeginDisabledGroup(this.workSpace.sourceGeometry != null);
        this.workSpace.sourceGeometry = (VLGeometry) EditorGUILayout.ObjectField(
            new GUIContent("Source Geometry", "The origin geometry (child of the VLWorkSpace)"),
            this.workSpace.sourceGeometry,
            typeof(VLGeometry),
            true);
        EditorGUI.EndDisabledGroup();
        this.workSpace.destinationGeometry = (GameObject) EditorGUILayout.ObjectField(
            new GUIContent(
                "Destination Geometry",
                "Use any GameObject from the scene to set the destination to its center or use the destination child of VLWorkSpace"),
            this.workSpace.destinationGeometry,
            typeof(GameObject),
            true);
        if (EditorGUI.EndChangeCheck())
        {
            UpdatePoses();
        }
    }

    private void DisplayViewDirections()
    {
        this.workSpace.displayViewDirection = EditorGUILayout.Toggle(
            new GUIContent(
                "Display View Direction",
                "Display dotted lines between all origin and destination points"),
            this.workSpace.displayViewDirection);
    }

    private void DisplayPreviewInCamera()
    {
        if (this.workSpace.poses == null || this.workSpace.poses.Length == 0 ||
            EditorApplication.isPlaying)
        {
            return;
        }

        this.workSpace.previewWithCamera = EditorGUILayout.Toggle(
            new GUIContent("Preview in Camera", "Enable pose preview in game view"),
            this.workSpace.previewWithCamera);

        if (this.workSpace.previewWithCamera)
        {
            EditorGUI.indentLevel++;

            this.workSpace.usedCamera = (Camera) EditorGUILayout.ObjectField(
                new GUIContent(
                    "Used Camera",
                    "Camera that is used to preview the AutoInit poses. " +
                    "Attention: Transform of this camera will be changed by this feature!"),
                this.workSpace.usedCamera,
                typeof(Camera),
                true);

            if (this.workSpace.usedCamera != null)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginVertical();
                this.workSpace.camSliderPosition = EditorGUILayout.IntSlider(
                    new GUIContent(
                        "Switch through Poses: ",
                        "Use this slider to switch the camera preview through your set poses"),
                    this.workSpace.camSliderPosition,
                    0,
                    this.workSpace.poses.Length - 1);
                EditorGUILayout.EndVertical();
                if (EditorGUI.EndChangeCheck())
                {
                    this.workSpace.usedCamera.enabled = false;
                    this.workSpace.usedCamera.enabled = true;
                    SetPreviewCamera(this.workSpace.camSliderPosition);
                }
            }

            EditorGUI.indentLevel--;
        }
    }

    private void DisplayHelpBox()
    {
        EditorGUILayout.Space();
        if (this.workSpace.poses != null && this.workSpace.poses.Length > 0)
        {
            EditorGUILayout.HelpBox(
                "Will generate around " + this.workSpace.poses.Length + " Poses" + 
                    (this.workSpace.poses.Length > 3000 ?
                         " Having more than 3.000 poses may lead to performance issues." :
                         ""),
                this.workSpace.poses.Length > 3000 ? MessageType.Warning : MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox(
                "Will generate 0 Poses. Please set a valid source and destination object.", MessageType.Error);
        }
        EditorGUILayout.Space();
    }

    /// <summary>
    /// Set preview camera to one of the poses regarding the camStepPosition.
    /// </summary>
    /// <param name="camStepPos">camera step index</param>
    private void SetPreviewCamera(int camStepPos)
    {
        if (this.workSpace.poses == null ||
            this.workSpace.poses.Length <= camStepPos ||
            this.workSpace.usedCamera == null)
        {
            return;
        }

        float[] q = this.workSpace.poses[camStepPos].q;
        float[] t = this.workSpace.poses[camStepPos].t;
        Quaternion orientation;
        Vector3 position;

        VLUnityCameraHelper.VLPoseToCamera(
            new Vector3(t[0], t[1], t[2]),
            new Quaternion(q[0], q[1], q[2], q[3]),
            out position, out orientation,
            VLUnityCameraHelper.FlipCoordinateSystemHandedness.Automatically);

        this.workSpace.usedCamera.transform.position = position;
        this.workSpace.usedCamera.transform.rotation = orientation;
    }

    private void ApplySerializedValues()
    {
        this.serializedObject.FindProperty("displayViewDirection").boolValue = this.workSpace.displayViewDirection;
        this.serializedObject.FindProperty("previewWithCamera").boolValue = this.workSpace.previewWithCamera;
        this.serializedObject.FindProperty("usedCamera").objectReferenceValue = this.workSpace.usedCamera;
        this.serializedObject.FindProperty("camSliderPosition").intValue = this.workSpace.camSliderPosition;

        this.serializedObject.FindProperty("sourceGeometry").objectReferenceValue = this.workSpace.sourceGeometry;
        this.serializedObject.FindProperty("destinationGeometry").objectReferenceValue = this.workSpace.destinationGeometry;

        this.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Draw source and destination points (PaintVertices).
    /// Draw Lines (PaintLines) if that option is enabled in WorkSpace.
    /// </summary>
    /// <param name="workSpace"></param>
    /// <param name="gizmoType"></param>
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected)]
    private static void DrawGizmos(VLWorkSpace workSpace, GizmoType gizmoType)
    {
        if (workSpace.sourceGeometry != null && 
            workSpace.sourceGeometry.currentMesh != null)
        {
            VLGeometryEditor.PaintVertices(
                workSpace.sourceGeometry.currentMesh, 
                Color.white, 
                workSpace.sourceGeometry.transform);
        }

        if (workSpace.displayViewDirection)
        {
            PaintLines(workSpace);
        }

        if (workSpace.destinationGeometry != null)
        {
            Vector3[] destinationVertices = workSpace.GetDestinationVertices();
            Handles.Label(destinationVertices[0], "Target");
            VLGeometryEditor.PaintVertices(
                destinationVertices, Color.cyan, workSpace.destinationGeometry.transform);
        }

        SceneView.RepaintAll();
    }

    /// <summary>
    /// Draw Lines from all source vertices to all destination vertices
    /// as long as the number of lines to draw is smaller than maxLinesToDraw.
    /// </summary>
    /// <param name="workSpace"></param>
    public static void PaintLines(VLWorkSpace workSpace)
    {
        if (workSpace == null || workSpace.sourceGeometry == null || workSpace.destinationGeometry == null ||
            workSpace.sourceGeometry.currentMesh == null)
        {
            return;
        }

        Vector3[] destinationVertices = workSpace.GetDestinationVertices();
        if (workSpace.sourceGeometry.currentMesh.Length * destinationVertices.Length > maxLinesToDraw)
        {
            return;
        }

        foreach (Vector3 sourcePos in workSpace.sourceGeometry.currentMesh)
        {
            foreach(Vector3 targetPos in destinationVertices)
            {
                Handles.DrawDottedLine(
                    workSpace.sourceGeometry.transform.TransformPoint(sourcePos),
                    workSpace.destinationGeometry.transform.TransformPoint(targetPos),
                    7f);
            }
        }
    }
}
