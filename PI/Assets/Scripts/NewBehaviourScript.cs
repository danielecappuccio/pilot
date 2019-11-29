using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTracker : MonoBehaviour
{
    private VLWorkerBehaviour worker;

    private void Start()
    {
        worker = GetComponent<VLWorkerBehaviour>();
        worker.StartTracking("Examples/ModelTracking/ModelTracker.vl");
    }

    private void OnDestroy()
    {
        worker.StopTracking();
    }
}