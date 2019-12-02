using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor script modifying and displaying relevant VLGeometry.
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(VLGeometry))]
public class VLGeometryEditor : Editor
{
    private VLGeometry geometry;
    private bool showHorizontalAngleConstraints = true;
    private bool showVerticalAngleConstraints = true;

    private static float wireSphereSize = 0.125f;
    
    private void Reset()
    {
        this.geometry = this.target as VLGeometry;
        UpdateGeometryMesh();
    }

    private void OnDisable()
    {
        ApplySerializedValues();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (this.geometry == null)
        {
            return;
        }

        EditorGUI.BeginChangeCheck();

        DisplayGeometryValues();

        if (EditorGUI.EndChangeCheck())
        {
            UpdateGeometryMesh();
            EditorUtility.SetDirty(this.geometry);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(this.geometry.gameObject.scene);
        }
    }

    private void DisplayGeometryValues()
    {
        DisplayGeometryShapeDropDown();

        EditorGUILayout.BeginVertical();
        switch (this.geometry.shape)
        {
            case VLGeometryShape.Sphere:
                DisplaySphereValues();
                break;
            case VLGeometryShape.Plane:
                DisplayPlaneValues();
                break;
            case VLGeometryShape.Line:
                DisplayLineValues();
                break;
            case VLGeometryShape.Point:
            default:
                break;
        }
        EditorGUILayout.EndVertical();
    }

    private void DisplayGeometryShapeDropDown()
    {
        SerializedProperty geometryShape = this.serializedObject.FindProperty("shape");
        geometryShape.enumValueIndex = (int)(VLGeometryShape) EditorGUILayout.EnumPopup(
            "Shape",
            (VLGeometryShape) System.Enum.GetValues(typeof(VLGeometryShape))
                .GetValue(geometryShape.enumValueIndex));

        this.serializedObject.ApplyModifiedProperties();
    }

    private void DisplaySphereValues()
    {
        this.geometry.sphereRadius =
            EditorGUILayout.FloatField("Radius: ", this.geometry.sphereRadius);

        this.geometry.detailLevel = EditorGUILayout.Slider(
            new GUIContent("Detail Level: ", "Effects the point count of the whole sphere"),
            this.geometry.detailLevel,
            0f,
            1f);

        this.showVerticalAngleConstraints = EditorGUILayout.Foldout(
            this.showVerticalAngleConstraints, "Vertical Angle Constraints (Polar)");
        if (this.showVerticalAngleConstraints)
        {
            this.geometry.thetaStart = EditorGUILayout.Slider(
                new GUIContent("Polar Start: ", "Vertical starting angle"),
                this.geometry.thetaStart,
                0f,
                180f);
            this.geometry.thetaLength = EditorGUILayout.Slider(
                new GUIContent("Polar Length: ", "Vertical sweep angle size"),
                this.geometry.thetaLength,
                0f,
                180f);
        }

        this.showHorizontalAngleConstraints = EditorGUILayout.Foldout(
            this.showHorizontalAngleConstraints, "Horizontal Angle Constraints (Azimuthal)");
        if (this.showHorizontalAngleConstraints)
        {
            this.geometry.phiStart = EditorGUILayout.Slider(
                new GUIContent("Azimuth Start: ", "Horizontal starting angle"),
                this.geometry.phiStart,
                0f,
                360f);
            this.geometry.phiLength = EditorGUILayout.Slider(
                new GUIContent("Azimuth Length: ", "Horizontal sweep angle size"),
                this.geometry.phiLength,
                0f,
                360f);
        }
    }

    private void DisplayPlaneValues()
    {
        this.geometry.planeLength =
            EditorGUILayout.FloatField("Length: ", this.geometry.planeLength);
        this.geometry.planeWidth =
            EditorGUILayout.FloatField("Width: ", this.geometry.planeWidth);
        this.geometry.planeStep = EditorGUILayout.IntField(
            new GUIContent("Steps: ", "Point count of one line of the plane"),
            this.geometry.planeStep);
    }

    private void DisplayLineValues()
    {
        this.geometry.lineLength =
            EditorGUILayout.FloatField("Length: ", this.geometry.lineLength);
        this.geometry.lineStep =
            EditorGUILayout.IntField(new GUIContent("Steps: ", "Point count of the line"), this.geometry.lineStep);
    }
    
    /// <summary>
    /// Updates the stored positions of the geometry.
    /// </summary>
    private void UpdateGeometryMesh()
    {
        this.geometry.UpdateMesh();
        ApplySerializedValues();
    }

    private void ApplySerializedValues()
    {
        if (this.geometry == null)
        {
            return;
        }

        this.serializedObject.FindProperty("lineLength").floatValue = this.geometry.lineLength;
        this.serializedObject.FindProperty("lineStep").intValue = this.geometry.lineStep;

        this.serializedObject.FindProperty("planeWidth").floatValue = this.geometry.planeWidth;
        this.serializedObject.FindProperty("planeLength").floatValue = this.geometry.planeLength;
        this.serializedObject.FindProperty("planeStep").intValue = this.geometry.planeStep;

        this.serializedObject.FindProperty("detailLevel").floatValue = this.geometry.detailLevel;
        this.serializedObject.FindProperty("sphereRadius").floatValue = this.geometry.sphereRadius;
        this.serializedObject.FindProperty("phiStart").floatValue = this.geometry.phiStart;
        this.serializedObject.FindProperty("phiLength").floatValue = this.geometry.phiLength;
        this.serializedObject.FindProperty("thetaStart").floatValue = this.geometry.thetaStart;
        this.serializedObject.FindProperty("thetaLength").floatValue = this.geometry.thetaLength;

        this.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Draw vertices if a geometry creator object is selected.
    /// Draw them white, if it's the Origin, blue if it's the Destination.
    /// </summary>
    /// <param name="geometry"></param>
    /// <param name="gizmoType"></param>
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected)]
    public static void DrawSingleSelectedGeometryGizmos(
        VLGeometry geometry,
        GizmoType gizmoType)
    {
        if (Selection.activeObject.name == "Origin")
        {
            PaintVertices(geometry.currentMesh, Color.white, geometry.transform);
        }
        else if (Selection.activeObject.name == "Destination")
        {
            PaintVertices(geometry.currentMesh, Color.cyan, geometry.transform);
        }
    }

    /// <summary>
    /// Draw wiresphere at given coordinates.
    /// </summary>
    /// <param name="vertices">Points to be drawn</param>
    /// <param name="paintColor">Gizmo color</param>
    /// <param name="trans">Transform applied to the points before drawing</param>
    public static void PaintVertices(Vector3[] vertices, Color paintColor, Transform trans)
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = paintColor;

        foreach(Vector3 vertPos in vertices)
        {
            Vector3 targetPosition = trans.TransformPoint(vertPos);

            Gizmos.DrawWireSphere(
                targetPosition,
                HandleUtility.GetHandleSize(targetPosition) * VLGeometryEditor.wireSphereSize);
        }
    }
}
