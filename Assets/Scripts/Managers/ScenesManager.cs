using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void ChangeScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Menu")
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }

        else
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
