using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListComponent : MonoBehaviour
{
    static private ListComponent selected = null;
    private Color colorStart;
    static private Camera detailCamera;

    public Color getColorStart()
    {
        return colorStart;
    }

    // Start is called before the first frame update
    void Start()
    {
        colorStart = gameObject.GetComponent<MeshRenderer>().material.color;
        foreach(Camera c in Camera.allCameras)
            if(c.gameObject.name == "DetailCamera")
            {
                detailCamera = c;
                c.enabled = false;
                break;
            }
    }

    private void OnMouseEnter()
    {
        if (this != selected)
            gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        if (this!= selected)
            gameObject.GetComponent<MeshRenderer>().material.color = colorStart;
    }

    private void OnMouseUpAsButton()
    {
        if (selected == null)
        {
            selected = this;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            detailCamera.enabled = true;
        }
        else if (selected == this)
        {
            selected = null;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            detailCamera.enabled = false;
        }
        else
        {
            selected.GetComponent<MeshRenderer>().material.color = selected.getColorStart();
            selected = this;
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
