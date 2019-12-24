using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StartTrackingWithCameraSelectionBehaviour))]
public class ListParent : MonoBehaviour
{

    [SerializeField]
    private string vlFile;
    [SerializeField]
    private string vlFileScrew;

    private ListComponent activeComponent = null;
    private List<ListComponent> components = new List<ListComponent>();
    [SerializeField]
    private TrackingAssembly     assembly;

    private bool lastScrew = false;

    public void addComponent(ListComponent listComponent)
    {
        components.Add(listComponent);
    }

    public void changeSelection(ListComponent selected)
    {
        if (activeComponent != null)
            activeComponent.SetState(false);
        activeComponent = selected;
        activeComponent.SetState(true);
        if (selected.isScrew && !lastScrew)
        {
            lastScrew = true;
            gameObject.GetComponent<StartTrackingWithCameraSelectionBehaviour>().StartTracking(vlFileScrew);
        }
        else if (!selected.isScrew && lastScrew)
        {
            lastScrew = false;
            gameObject.GetComponent<StartTrackingWithCameraSelectionBehaviour>().StartTracking(vlFile);
        }
    }

    public void changeSelection(TrackingAssembly selected)
    {
        if (activeComponent != null)
            activeComponent.SetState(false);
        activeComponent = null;
        selected.gameObject.SetActive(true);
        selected.transform.rotation = Quaternion.identity;
        selected.GetComponent<VLModelTrackableBehaviour>().UpdateTransformation(true);
        if(lastScrew)
            gameObject.GetComponent<StartTrackingWithCameraSelectionBehaviour>().StartTracking(vlFile);
    }

    public void recognizeNext()
    {
        foreach (ListComponent x in components)
        {
            if (!x.GetRecognized())
            {
                changeSelection(x);
                return;
            }
        }

        if (!FindObjectOfType<TaskImage>().getOpen())
            FindObjectOfType<TaskImage>().TaskSelect();
        changeSelection(assembly);
    }

    private void Start()
    {
        gameObject.GetComponent<StartTrackingWithCameraSelectionBehaviour>().StartTracking(vlFile);
    }
}
