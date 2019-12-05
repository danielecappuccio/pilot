using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StartTrackingWithCameraSelectionBehaviour))]
public class ListParent : MonoBehaviour
{

    [SerializeField]
    private string vlFile;

    private ListComponent activeComponent = null;

    public void changeSelection(ListComponent selected)
    {
        if (activeComponent != null)
            activeComponent.SetState(false);
        activeComponent = selected;
        activeComponent.SetState(true);
    }

    private void Start()
    {
        gameObject.GetComponent<StartTrackingWithCameraSelectionBehaviour>().StartTracking(vlFile);
    }
}
