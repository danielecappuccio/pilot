using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
    This piece of code helps understanding how parts can be switched on and off.
    Here we switch through parts, which we asssume to have the same name as in the defined .vl file.
    Please checkout the legoExample folder in the StreamingAssets/VisionLib.

    SetModelProperty is the most important function.
*/

public class PartSwitcherBehaviourScript : MonoBehaviour
{
    // Names of the objects defined in the vl file
    public string[] objects;

    // VLWorkerBehaviour used in this tracking scenario
    public VLModelTrackerBehaviour_v1 modelTrackerBehaviour;

    /// Enable for recognition of update ( this will only work on significant changes in the tracking target!)
    public bool autoInstructionUpdate = false;


    // GameObject containing the objects, which should be showed, as children
    public GameObject father;

    // Material for pieces, which should be added in the current step
    public Material guidedMat;

    // Material for pieces, which have already been added
    public Material placedMat;

    private int curStep = 0;
    private int trackingCounter = 0;

    private void prepareStep(int step)
    {
        if (step > objects.Length){
            return;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            bool enabledState = false; // enable object for tracking    
            bool showObject = false; // show object in GUI
            bool showGuide = false;  // show object in GUI with guide material or placed material
            if (i < step)
            {
                // set
                enabledState = true;
                showObject = true;
            }
            else if (i == step)
            {
                // show instructions
                enabledState = false;
                showObject = true;
                showGuide = true;
            }

            // We enable each object in the visionlib corresponding to the building state by setting the enabled attribute
            modelTrackerBehaviour.SetModelProperty("name:" + objects[i], "enabled", enabledState);

            // We set the material and the visiblity for the user
            GameObject Go = this.father.transform.GetChild(i).gameObject;
            Go.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = showGuide ? guidedMat : placedMat;
            Go.SetActive(showObject);

        }

        // just for info purposes, retreive the current model properties (not needed)
        modelTrackerBehaviour.GetModelProperties(); 
    }
    
    public void NextStep()
    {
        curStep++;
        if (curStep > objects.Length) curStep = 0;
        prepareStep(curStep);

    }

    void OnTrackerInitialized(bool success)
    {
        // We start everything, when the tracker has been initialized.
        if (success) 
        {
            curStep = 0;
            prepareStep(curStep);
        }
    }

    void AutoUpdateInstructions(VLTrackingState states)
    {
		if (!autoInstructionUpdate) return;

        // We start everything, when the tracker has been initialized.
        foreach(VLTrackingState.TrackingObject trackingObject in states.objects)
        {
            // Count how many frames the object has been tracked in a row
            if (trackingObject.state == "tracked")
            {
                trackingCounter++;
            }
            else
            {
                // If tracking is lost or critical, reset the tracking counter
                trackingCounter = 0;
            }

            // If object has been tracked for 60 frames, go to the next 
            // instruction step
            if (trackingCounter == 60)
            {
                trackingCounter = 0;
                if (curStep < objects.Length)
                {
                    NextStep();
                }
            }
        }
    }

    void LogModelProperties(VLModelProperties[] properties)
    {
         Debug.Log("Got model properties: " + properties.Length);
         for (var i =0;i<properties.Length;i++)
         {
             Debug.Log("Model:"+i+" with name:"+properties[i].name+" is enabled:" + properties[i].enabled);

         }
    }
    
    void OnEnable()
    {
        VLModelTrackerBehaviour_v1.OnGetModelProperties += LogModelProperties;
        VLWorkerBehaviour.OnTrackerInitialized +=OnTrackerInitialized;
        VLWorkerBehaviour.OnTrackingStates += AutoUpdateInstructions;
    }

    void OnDisable()
    {
        VLWorkerBehaviour.OnTrackingStates -= AutoUpdateInstructions;
        VLWorkerBehaviour.OnTrackerInitialized -= OnTrackerInitialized;
        VLModelTrackerBehaviour_v1.OnGetModelProperties -= LogModelProperties;
    }

    void OnGUI(){
        if (curStep >= objects.Length)
        {
            GUI.Label(new Rect(200, 150, 300, 30), "Finished");
        }
        else
        {
            GUI.Label(new Rect(200, 150, 300, 30), "Step:" + (curStep + 1) + "/" + (objects.Length + 1));
        }
    }
}
