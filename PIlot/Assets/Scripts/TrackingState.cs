using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingState : MonoBehaviour
{
    [SerializeField]
    private int TARGET = 30;

    private string state;
    private int counter = 0;

    [SerializeField]
    private ListComponent listComponent;
    [SerializeField]
    private ListComponent next;
    [SerializeField]
    private ListParent parent;


    void FixedUpdate()
    {
        state = gameObject.GetComponent<VLTrackedObjectBehaviour>().previousState;

        switch (state)
        {
            case "tracked":
                counter++;
                break;
            case "critical":
                break;
            case "lost":
                counter = 0;
                break;
            default:
                counter = 0;
                break;
        }

        if (counter == TARGET)
        {
            listComponent.SetRecognized(true);
            parent.changeSelection(next);
        }
    }
}
