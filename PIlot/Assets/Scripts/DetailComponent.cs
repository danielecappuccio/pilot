using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailComponent : MonoBehaviour
{

    private Vector3 mouseReference;
    private float sensitivityFactor = 0.01f;

    [SerializeField]
    private int dragSensitivity;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        mouseReference = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector3 offset = Input.mousePosition - mouseReference;
        gameObject.transform.Rotate(sensitivityFactor * dragSensitivity * offset.y, -sensitivityFactor * dragSensitivity * offset.x, 0, Space.World);
        mouseReference = Input.mousePosition;
    }
}
