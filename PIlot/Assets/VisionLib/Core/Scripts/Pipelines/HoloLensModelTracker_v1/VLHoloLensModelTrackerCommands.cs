/** @addtogroup vlUnitySDK
 *  @{
 */

using System;
using VLWorkerCommands;

/// <summary>
///  Namespace with command classes for communicating with the model-based
///  tracking under HoloLens
/// </summary>
namespace VLHoloLensModelTrackerCommands
{
    /// <summary>
    ///  Set the global pose of the object to track.
    /// </summary>
    [Serializable]
    public class SetGlobalObjectPoseCmd : VLWorkerCommands.CommandBase
    {
        private static readonly string defaultName = "setGlobalObjectPose";

        [Serializable]
        public class Param
        {
            public float[] t;
            public float[] q;
            public Param(float tx, float ty, float tz,
                float qx, float qy, float qz, float qw)
            {
                this.t = new float[3] {
                    tx,
                    ty,
                    tz
                };
                this.q = new float[4] {
                    qx,
                    qy,
                    qz,
                    qw
                };
            }
        }

        public Param param;
        public SetGlobalObjectPoseCmd(Param param) : base(defaultName)
        {
            this.param = param;
        }
    }

    /// <summary>
    ///  Set the global coordinate system of Unity to be used in the vlSDK
    /// </summary>
    [Serializable]
    public class SetGlobalCoordinateSystemCommandDescription
    {
        private static readonly string defaultName =
            "setGlobalCoordinateSystem";

        public string name;

        public SetGlobalCoordinateSystemCommandDescription()
        {
            this.name = defaultName;
        }
    }

} // namespace VLHoloLensModelTrackerCommands

/**@}*/
