using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingParent : MonoBehaviour
{
    private GameObject tracking = null;

    public void setTracking(GameObject obj)
    {
        if (obj == null)
            return;

        if (tracking != null)
            tracking.SetActive(false);
        tracking = obj;
        obj.SetActive(true);
    }

    public void resetTracking()
    {
        if (tracking != null)
            tracking.SetActive(false);
        tracking = null;
    }
}
