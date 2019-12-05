using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingState : MonoBehaviour
{
    private int TARGET = 30;
    private string state;
    private int counter = 0;

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
            print("Tracked");
            // TODO
        }
    }
}
