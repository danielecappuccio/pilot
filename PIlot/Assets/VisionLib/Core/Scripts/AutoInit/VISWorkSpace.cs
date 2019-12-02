/** @addtogroup vlUnitySDK
 *  @{
 */

using System;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VIS
{
/// <summary>
///  Namespace with classes for generating WorkSpace json. These will be used for calling
///  corresponding commands in the vlSDK.
///  **THIS IS SUBJECT TO CHANGE** Please do not rely on this code in productive environments.
/// </summary>
namespace WorkSpace
{
/// <summary>
///  Definition of a transformation with quaternion and translation
/// </summary>
[Serializable]
public class Transform
{
    public float[] t;
    public float[] q;

    public Transform()
    {
        this.t = new float[3]{0, 0, 0};
        this.q = new float[4]{0, 0, 0, 1};
    }

    public Transform(Vector3 translation, Quaternion rotation)
    {
        this.t = new float[3]{translation.x, translation.y, translation.z};
        this.q = new float[4]{rotation.x, rotation.y, rotation.z, rotation.w};
    }
}

/// <summary>
///  Base type of WorkSpace geometries
/// </summary>
[Serializable]
public class Geometry
{
    /// <summary>
    ///  Parameters of WorkSpace geometries
    /// </summary>
    [Serializable]
    public class Parameters
    {
        public Transform transformation;

        public int planeSteps;
        public float planeLength;
        public float planeWidth;

        public int sphereSamples;
        public float distance;
        public float sphereRadius;
        public float sphereThetaStart;
        public float sphereThetaLength;
        public float spherePhiStart;
        public float spherePhiLength;
    }

    public string type;
    public Parameters parameters;

    public Geometry(string typeName)
    {
        this.type = typeName;
    }

    [DllImport(VLUnitySdk.dllName)]
    private static extern IntPtr vlSDKUtil_getCameraPositionsFromGeometry(
        [MarshalAs(UnmanagedType.LPStr)] string baseNodeJson,
        out int size);

    /// <summary>
    /// Function for obtaining all positions of a geometry definition directly from vlSDK.
    /// </summary>
    /// <returns>Array of Unity coordinates, which are described by the given Geometry</returns>
    public Vector3[] GetCameraPositions()
    {
        int positionsSize = 0;
        IntPtr positions =
            vlSDKUtil_getCameraPositionsFromGeometry(VLJsonUtility.ToJson(this), out positionsSize);

        if (positions == IntPtr.Zero)
        {
            return new Vector3[0];
        }

        float[] positionsVector = new float[positionsSize * 3];
        Marshal.Copy(positions, positionsVector, 0, positionsSize * 3);
        Marshal.FreeHGlobal(positions);

        Vector3[] cameraPositions = new Vector3[positionsSize];

        for (int i = 0; i < positionsSize; i++)
        {
            cameraPositions[i] = VLUnityCameraHelper.VLPoseToUnityPose(new Vector3(
                positionsVector[i * 3], positionsVector[i * 3 + 1], positionsVector[i * 3 + 2]));
        }

        return cameraPositions;
    }
}

/// <summary>
///  WorkSpace Geometry describing a plane in x (width) and y (length) direction.
/// </summary>
[Serializable]
public class Plane : Geometry
{
    private static readonly string defaultTypeName = "plane";

    public Plane() : base(defaultTypeName)
    {
        this.parameters = new Parameters();
        this.parameters.transformation = new Transform();

        this.parameters.planeSteps = 1;
        this.parameters.planeLength = 0;
        this.parameters.planeWidth = 0;
    }

    public Plane(float length, float width, int steps, WorkSpace.Transform trans) :
        base(defaultTypeName)
    {
        this.parameters = new Parameters();

        this.parameters.planeLength = length;
        this.parameters.planeWidth = width;
        this.parameters.planeSteps = steps;

        this.parameters.transformation = trans;
    }
}

/// <summary>
///  WorkSpace Geometry describing a sphere.
///  Phi describes the polar angle in degree around the x axis.
///  Theta describes the azimuth angle around the z axis
/// </summary>
[Serializable]
public class Sphere : Geometry
{
    private static readonly string defaultTypeName = "sphere";

    public Sphere() : base(defaultTypeName)
    {
        this.parameters = new Parameters();
        this.parameters.transformation = new Transform();

        this.parameters.sphereSamples = 100;
        this.parameters.distance = 0;
        this.parameters.sphereRadius = 1;
        this.parameters.sphereThetaStart = 0;
        this.parameters.sphereThetaLength = 360;
        this.parameters.spherePhiStart = 0;
        this.parameters.spherePhiLength = 180;
    }

    public Sphere(VLGeometry usedGeometry, WorkSpace.Transform trans) : base(defaultTypeName)
    {
        this.parameters = new Parameters();
        this.parameters.sphereRadius = usedGeometry.sphereRadius;
        this.parameters.sphereThetaStart = usedGeometry.thetaStart;
        this.parameters.sphereThetaLength = usedGeometry.thetaLength;
        this.parameters.sphereSamples = 
            Mathf.FloorToInt(Remap(usedGeometry.detailLevel, 0f, 1f, 42f, 1281f));
        this.parameters.spherePhiStart = usedGeometry.phiStart;
        this.parameters.spherePhiLength = usedGeometry.phiLength;

        this.parameters.transformation = trans;
    }

    private float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        float fromAbs = from - fromMin;
        float fromMaxAbs = fromMax - fromMin;

        float normal = fromAbs / fromMaxAbs;

        float toMaxAbs = toMax - toMin;
        float toAbs = toMaxAbs * normal;

        float to = toAbs + toMin;

        return to;
    }

}

/// <summary>
///  Definition of a single workspace.
///  It contains one origin and one destination geometry from which the possible poses will be
///  calculated. Additionally, the upVector and the rotation values define the possible view
///  angles of the camera.
/// </summary>
[Serializable]
public class Definition
{
    public string type = "WorkSpaceDef";
    public Transform transformation;
    public float rollAngleRange;
    public float rollAngleStep;
    public float[] upVector;

    [SerializeField]
    public Geometry origin;

    [SerializeField]
    public Geometry destination;

    public Definition()
    {
        this.transformation = new Transform();
        this.rollAngleRange = 0;
        this.rollAngleStep = 0;
        this.upVector = new float[3]{0, 1, 0};
    }
    public Definition(Transform trans, Vector3 upVector, float rollAngleRange, float rollAngleStep)
    {
        this.transformation = trans;
        this.rollAngleRange = rollAngleRange;
        this.rollAngleStep = rollAngleStep;
        this.upVector = new float[3]{upVector.x, upVector.y, upVector.z};
    }

    [DllImport(VLUnitySdk.dllName)]
    private static extern IntPtr vlSDKUtil_getCameraTransformsFromWorkspaceDefinition(
        [MarshalAs(UnmanagedType.LPStr)] string workspaceJson,
        out int size);

    /// <summary>
    /// Function for obtaining all poses of a WorkSpace definition directly from vlSDK.
    /// </summary>
    /// <returns>
    /// Array of Workspace.Transform in the vlSDK coordinate system, which represent all
    /// camera poses described by this WorkSpace.Definition
    /// </returns>
    public Transform[] GetCameraTransforms()
    {
        int transformsSize = 0;
        IntPtr transforms = vlSDKUtil_getCameraTransformsFromWorkspaceDefinition(
            VLJsonUtility.ToJson(this), out transformsSize);

        if (transforms == IntPtr.Zero)
        {
            return new WorkSpace.Transform[0];
        }

        float[] transformsVector = new float[transformsSize * 7];
        Marshal.Copy(transforms, transformsVector, 0, transformsSize * 7);
        Marshal.FreeHGlobal(transforms);

        WorkSpace.Transform[] result = new WorkSpace.Transform[transformsSize];

        for (int i = 0; i < transformsSize; i++)
        {
            result[i] = new WorkSpace.Transform(
                new Vector3(
                    transformsVector[i * 7 + 0],
                    transformsVector[i * 7 + 1],
                    transformsVector[i * 7 + 2]),
                new Quaternion(
                    transformsVector[i * 7 + 3],
                    transformsVector[i * 7 + 4],
                    transformsVector[i * 7 + 5],
                    transformsVector[i * 7 + 6]));
        }

        return result;
    }
}

/// <summary>
/// WorkSpace Configuration for a scene.
/// It contains all WorkSpaces of the Scene, which should be used for AutoInit.
/// </summary>
[Serializable]
public class Configuration
{
    public string type = "VisionLibWorkSpacesConfig";
    public int version = 1;

    public WorkSpace.Definition[] workSpaces;

    public Configuration(WorkSpace.Definition[] workSpaces)
    {
        this.workSpaces = workSpaces;
    }

    /// <summary>
    /// Writes this WorkSpace.Configuration into the specified file.
    /// </summary>
    /// <param name="fileName">Path of the file to write the data in.</param>
    /// <remarks>
    ///  <para>
    ///   It's possible to use vlSDK file schemes (e.g. local_storage_dir) here.
    ///  </para>
    /// </remarks>
    public void WriteToFile(string fileName)
    {
        VLUnitySdk.Set(fileName, VLJsonUtility.ToJson(this), "");
    }
}
} // namespace WorkSpaceStructs
} // namespace VIS

/**@}*/
