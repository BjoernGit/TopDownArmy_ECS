using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAfterStartup : MonoBehaviour
{
    public string sceneAfterStartup;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SceneSwitcher>().SwitchScene(sceneAfterStartup);
    }

}
