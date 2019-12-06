using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListComponent : MonoBehaviour
{
    [SerializeField]
    private RawImage image;

    public bool isSelected = false;
    private bool isRecognized = false;

    [SerializeField]
    private Color selectedColor = Color.yellow;
    [SerializeField]
    private Color unselectedColor = Color.red;
    [SerializeField]
    private Color recognizedColor = Color.green;

    [SerializeField]
    private GameObject trackableObj;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponentInParent<ListParent>().addComponent(this);
        bool tmp = isSelected;
        this.SetState(false);
        if (tmp)
            this.ChangeState();
    }

    public void SetRecognized()
    {
        isRecognized = true;
        image.color = recognizedColor;
        trackableObj.SetActive(false);
        this.GetComponentInParent<ListParent>().recognizeNext();
    }

    public bool GetRecognized()
    {
        return isRecognized;
    }

    public void SetState(bool selected)
    {
        if (isRecognized)
            return;

        isSelected = selected;
        if (isSelected)
        {
            image.color = selectedColor;
            
            trackableObj.SetActive(true);
            trackableObj.transform.rotation = Quaternion.identity;
            trackableObj.GetComponent<VLModelTrackableBehaviour>().UpdateTransformation(true);
        }
        else
        {
            image.color = unselectedColor;
            trackableObj.SetActive(false);
        }

    }

    public void ChangeState()
    {
        if (!isRecognized)
        {
            this.GetComponentInParent<ListParent>().changeSelection(this);
        }
    }
}
