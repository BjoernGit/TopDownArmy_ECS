using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelToSelect : MonoBehaviour
{
    public string sceneName;



    public void StartLevel()
    {
        GameObject.Find("SceneSwitcher").GetComponent<SceneSwitcher>().SwitchScene(sceneName);
    }


}
