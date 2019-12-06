using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackingAssembly : MonoBehaviour
{
    [SerializeField]
    private int TARGET = 30;

    private string state;
    private int counter = 0;

    private SceneChanger sceneChanger;
    [SerializeField]
    private int scene;

    private void Start()
    {
        sceneChanger = GetComponent<SceneChanger>();
    }

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
            sceneChanger.changeScene(scene);
    }
}
