using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCalibrationExampleVisualizeTrackingState : MonoBehaviour
{
    public GameObject pattern;

    void Start ()
    {
    }

    void Update ()
    {
    }

    void StoreTrackingStates(VLTrackingState state)
    {
        if (state.objects.Length == 0)
        {
            return;
        }

        if (this.pattern == null)
        {
            return;
        }

        VLTrackingState.TrackingObject obj = state.objects[0];
        if (obj.state == "collectingActive")
        {
            this.pattern.SetActive(true);
        }
        else
        {
            this.pattern.SetActive(false);
        }
    }

    void OnEnable()
    {
        VLWorkerBehaviour.OnTrackingStates += StoreTrackingStates;
    }

    void OnDisable()
    {
        VLWorkerBehaviour.OnTrackingStates -= StoreTrackingStates;
    }

}
