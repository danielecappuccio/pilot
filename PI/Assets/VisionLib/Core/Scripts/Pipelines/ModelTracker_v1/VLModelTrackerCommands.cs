/** @addtogroup vlUnitySDK
 *  @{
 */

using System;
using VLWorkerCommands;

/// <summary>
///  Namespace with command classes for communicating with the model-based
///  tracking.
/// </summary>
namespace VLModelTrackerCommands
{
    /// <summary>
    ///  Get the initial pose.
    /// </summary>
    [Serializable]
    public class GetInitPoseCmd : CommandBase
    {
        private static readonly string defaultName = "getInitPose";
        public GetInitPoseCmd() : base(defaultName) {}
    }

    /// <summary>
    ///  Result of GetInitPoseCmd.
    /// </summary>
    [Serializable]
    public struct GetInitPoseResult
    {
        public float[] t;
        public float[] q;
    }

    /// <summary>
    ///  Set the initial pose.
    /// </summary>
    [Serializable]
    public class SetInitPoseCmd : CommandBase
    {
        private static readonly string defaultName = "setInitPose";

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
        public SetInitPoseCmd(Param param) : base(defaultName)
        {
            this.param = param;
        }
    }

    /// <summary>
    ///  Write init data to default location with default name.
    /// </summary>
    [Serializable]
    public class WriteInitDataCmd : CommandBase
    {
        private static readonly string defaultName = "writeInitData";
        public WriteInitDataCmd() : base(defaultName) {}
    }

    /// <summary>
    ///  Write init data to custom location with custom file name.
    /// </summary>
    [Serializable]
    public class WriteInitDataWithPrefixCmd : CommandBase
    {
        private static readonly string defaultName = "writeInitData";
        public string param;
        public WriteInitDataWithPrefixCmd(string filePrefix) : base(defaultName)
        {
            this.param = filePrefix;
        }
    }

    /// <summary>
    ///  Read init data from custom location with custom file name.
    /// </summary>
    [Serializable]
    public class ReadInitDataWithPrefixCmd : CommandBase
    {
        private static readonly string defaultName = "readInitData";
        public string param;
        public ReadInitDataWithPrefixCmd(string filePrefix) : base(defaultName)
        {
            this.param = filePrefix;
        }
    }

     /// <summary>
    ///  Reset Offline init data 
    /// </summary>
    [Serializable]
    public class ResetInitDataCmd : CommandBase
    {
        private static readonly string defaultName = "resetInitData";
        public ResetInitDataCmd() : base(defaultName) {}
    }


    [Serializable]
    public class SetModelBoolPropertyCmd : CommandBase
    {
        private static readonly string defaultName = "setModelProperty";

        [Serializable]
        public struct Param
        {
            public string name;
            public string property;
            public bool value;
            public Param(string name, string property, bool enable)
            {
                this.name = name;
                this.property = property;
                this.value = enable;
            }
        }

        public Param param;
        public SetModelBoolPropertyCmd(string name, string property, bool enable) : base(defaultName)
        {
            this.param = new Param(name, property, enable);
        }
    }

    [Serializable]
    public class GetModelPropertiesCmd : CommandBase
    {
        private static readonly string defaultName = "getModelProperties";

        public GetModelPropertiesCmd() : base(defaultName) {}
    }

    [Serializable]
    public class RemoveModelCmd : CommandBase
    {
        private static readonly string defaultName = "removeModel";
        
        public string param;

        public RemoveModelCmd(string modelName) : base(defaultName)
        {
            this.param = modelName;
        }
    }
    
    /// <summary>
    ///  Set the WorkSpaces for AutoInit.
    /// </summary>
    [Serializable]
    public class SetWorkSpacesCmd : CommandBase
    {
        private static readonly string defaultName = "setWorkSpaces";

        public VIS.WorkSpace.Configuration param;

        public SetWorkSpacesCmd(VIS.WorkSpace.Configuration config) : base(defaultName)
        {
            this.param = config;
        }
    }
} // namespace VLModelTrackerCommands

/**@}*/