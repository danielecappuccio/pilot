using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the type of a VLGeometry. The corresponding parameters will be used and set in the 
/// corresponding VLGeometry.
/// </summary>
[System.Serializable]
public enum VLGeometryShape
{
    Sphere,
    Plane,
    Line,
    Point
}

/// <summary>
/// Data class for storing the values of different geometries for AutoInit. A VLWorkspace.Geometry
/// can be generated from this, which can be used in the vlSDK.
///  **THIS IS SUBJECT TO CHANGE** Please do not rely on this code in productive environments.
/// </summary>
[AddComponentMenu("VisionLib/AutoInit/VL Geometry")]
public class VLGeometry : MonoBehaviour
{
    [HideInInspector]
    public VLGeometryShape shape;

    // line parameters
    [HideInInspector]
    public float lineLength = 1f;
    [HideInInspector]
    public int lineStep = 3;

    // plane parameters
    [HideInInspector]
    public float planeLength = 1f;
    [HideInInspector]
    public float planeWidth = 1f;
    [HideInInspector]
    public int planeStep = 3;
    
    // sphere parameters
    [HideInInspector]
    public float sphereRadius = 1;
    [HideInInspector]
    public float detailLevel = 0.1f;
    [HideInInspector]
    public float phiStart = 0f;
    [HideInInspector]
    public float phiLength = 180f;
    [HideInInspector]
    public float thetaStart = 0f;
    [HideInInspector]
    public float thetaLength = 360f;

    [HideInInspector]
    [System.NonSerialized]
    public Vector3[] currentMesh;

    /// <summary>
    /// Creates a corresponding VLWorkSpace.Geometry, which can be used in the vlSDK.
    /// </summary>
    /// <param name="trans">Transform, which will be applied to the Geometry</param>
    /// <returns></returns>
    public VIS.WorkSpace.Geometry CreateVLGeometry(VIS.WorkSpace.Transform trans)
    {
        switch (this.shape)
        {
            case VLGeometryShape.Sphere:

                return new VIS.WorkSpace.Sphere(
                    this,
                    trans);

            case VLGeometryShape.Plane:

                return new VIS.WorkSpace.Plane(
                    this.planeLength,
                    this.planeWidth,
                    this.planeStep,
                    trans);

            case VLGeometryShape.Line:

                return new VIS.WorkSpace.Plane(
                    this.lineLength,
                    0,
                    this.lineStep,
                    trans);

            case VLGeometryShape.Point:
            default:

                return new VIS.WorkSpace.Plane(
                    0,
                    0,
                    1,
                    trans);
        }
    }

    /// <summary>
    /// Updates the stored positions of the geometry by the values taken from vlSDK.
    /// </summary>
    public void UpdateMesh()
    {
        VIS.WorkSpace.Geometry geometry = this.CreateVLGeometry(new VIS.WorkSpace.Transform());
        this.currentMesh = geometry.GetCameraPositions();
    }
}
