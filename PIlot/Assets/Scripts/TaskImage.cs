using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskImage : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    private bool isOpen = false;

    public bool getOpen()
    {
        return isOpen;
    }

    public void TaskSelect()
    {
        if (isOpen)
        {
            transform.Translate(new Vector3(camera.pixelWidth, 0, 0));
            camera.gameObject.SetActive(false);
            this.isOpen = false;
        }
        else
        {
            transform.Translate(new Vector3(-camera.pixelWidth, 0, 0));
            camera.gameObject.SetActive(true);
            this.isOpen = true;
        }
    }
}
