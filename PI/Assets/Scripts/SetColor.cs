using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{

    [SerializeField]
    private Color colorToSet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseOver()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    private void OnMouseDown()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = colorToSet;
    }
}
