/** @addtogroup vlUnitySDK
 *  @{
 */

using System;

/// <summary>
///  VLModelDeserializationResult stores associated data of the models.
/// <remarks>
/// <para>
///  VLModelDeserializationResult stores associated data of the models, which have been
///  added to tracking system. It contains, name, id and licensing informations.
///  Please NOTE: THIS PIECE OF CODE IS CONSIDERED AS BETA AND MAY BE MATTER OF CHANGE.
///  </para>
/// </remarks>
/// </summary>
[Serializable]
public class VLModelDeserializationResult
{
   
    /// <summary>
    /// The name of the model added to the tracking system.
    /// </summary>
    public string name;
    
    /// <summary>
    /// The internal id that has been assigned to the added model.
    /// </summary>
    public string id;

    /// <summary>
    /// Model hash of the data added to the tracking system. These strings can
    /// be used to acquire new license features for these models. 
    /// </summary>
    public string [] licenseFeatures;
}

/// <summary>
///  VLModelDeserializationStructure stores the internal states of the model which 
///  is used for model based tracking. Such an object is usually passed, when
///  calling VLWorker.PushJsonAndBinaryCommand with an addModelData command.
/// </summary>
[Serializable]
public class VLModelDeserializationStructure
{
    public VLModelDeserializationResult[] result;
    public string error;
}

/**@}*/
