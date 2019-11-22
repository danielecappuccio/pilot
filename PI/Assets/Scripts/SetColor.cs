using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{

    [SerializeField]
    private Color colorToSet;

    private Color colorOriginal;
    private bool isRotating;

    // Start is called before the first frame update
    void Start()
    {
        colorOriginal = gameObject.GetComponent<MeshRenderer>().material.color;
        isRotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, isRotating ? 100 * Time.deltaTime : 0, 0);
    }

    private void OnMouseEnter()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = colorToSet;
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = colorOriginal;
    }

    private void OnMouseDown()
    {
        isRotating = !isRotating;
    }
}
