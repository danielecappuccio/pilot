using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningLogo : MonoBehaviour
{
    public RawImage image;

    public void ShowWarning()
    {
        image.gameObject.SetActive(true);
    }

    public void RemoveWarning()
    {
        image.gameObject.SetActive(false);
    }
}
