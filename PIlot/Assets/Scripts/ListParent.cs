using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StartTrackingWithCameraSelectionBehaviour))]
public class ListParent : MonoBehaviour
{

    [SerializeField]
    private string vlFile;

    private ListComponent activeComponent = null;
    private List<ListComponent> components = new List<ListComponent>();
    [SerializeField]
    private ListComponent assembly;

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

        changeSelection(assembly);
    }

    private void Start()
    {
        gameObject.GetComponent<StartTrackingWithCameraSelectionBehaviour>().StartTracking(vlFile);
    }
}
