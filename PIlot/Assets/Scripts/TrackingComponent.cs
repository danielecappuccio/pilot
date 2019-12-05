using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingComponent : MonoBehaviour
{

    private Vector3 mouseReference;

    [SerializeField]
    private float dragSensitivity = .3f;

    private void OnMouseDown()
    {
        mouseReference = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector3 offset = Input.mousePosition - mouseReference;
        gameObject.transform.Rotate(dragSensitivity * offset.y, - dragSensitivity * offset.x, 0, Space.World);
        mouseReference = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        gameObject.GetComponent<VLModelTrackableBehaviour>().UpdateTransformation(true);
    }
}
