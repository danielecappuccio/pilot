using System;
using UnityEngine;

/// <summary>
///  Enables switching between several models.
/// </summary>
public class ChangeModelBehaviour : MonoBehaviour
{
    [System.Serializable]
    public class ModelURI
    {
        public string modelURI;
        public GameObject gameModel;
    }

    public VLModelTrackerBehaviour_v1 modelTrackerBehaviour;
    public ModelURI[] modelURIs;

    // Index of activeModel
    private int activeModelIndex = 0;

    /// <summary>
    ///  Activates the model specified by the index in Unity and the vlSDK.
    ///  The tracking will be reset after setting the new model.
    /// </summary>
    /// <param name="modelIndex">
    ///  Index of the model, which should be activated. The index has to be 
    ///  between 0 and modelURIs.Lenght
    /// </param>
    public void SetModel(int modelIndex)
    {
        if (modelIndex < 0 || modelIndex >= this.modelURIs.Length)
        {
            return;
        }

        this.activeModelIndex = modelIndex;

        // Only active model is visible
        for (int i = 0; i < this.modelURIs.Length; i++)
        {
            this.modelURIs[i].gameModel.SetActive(i == this.activeModelIndex);
        }

        // Set modelURI to active model
        this.modelTrackerBehaviour.SetModelURI(
            this.modelURIs[this.activeModelIndex].modelURI);

        // Reset tracking, so new model can now be tracked
        this.modelTrackerBehaviour.ResetTrackingHard();
    }

    /// <summary>
    ///  Switches to the next model in the modelURIs array.
    /// </summary>
    public void NextModel()
    {
        this.activeModelIndex++;
        if (this.activeModelIndex >= this.modelURIs.Length)
        {
            this.activeModelIndex = 0;
        }

        SetModel(this.activeModelIndex);
    }

}