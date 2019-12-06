using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public void changeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void setUID(Text uid)
    {
        if (uid.text == "" || uid.text == null)
            UserID.name = "User";
        else
            UserID.name = uid.text;
    }
}
