using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    private void Start()
    {
        //Tell GameManager that this scene is new
        GameManager.instance.ResetParametersForNewScene();
    }

    public void SwitchScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }
}
