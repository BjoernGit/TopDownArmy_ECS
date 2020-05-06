using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;

    private SceneSwitcher sceneSwitcher;
    private bool gameIsOver;

    //---Singleton-Pattern-----------------------
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SpawnManager in scene!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        GameObject.DontDestroyOnLoad(gameObject);
    }
    //---Singleton-Pattern-ENDE------------------

    private void Start()
    {
        sceneSwitcher = GameObject.Find("SceneSwitcher").GetComponent<SceneSwitcher>();
    }

    public void ResetParametersForNewScene()
    {
        //Do stuff to reset

        //Reset Spawnmanager Borders
        SpawnManager.instance.ResetBorderPositions();
        SpawnManager.instance.ResetAllPlayers();
    }

    public void RequestPlayerJoinDisabled()
    {
        GameObject.Find("PlayerInputManager").GetComponent<PlayerInputManager>().DisableJoining();
    }

    public void RequestPlayerJoinEnabled()
    {
        GameObject.Find("PlayerInputManager").GetComponent<PlayerInputManager>().EnableJoining();
    }

    public void GameOver()
    {
        if (!gameIsOver)
        {
            gameOverCanvas.SetActive(true);
            StartCoroutine("SwitchToJoinGame");
            gameIsOver = true;
        }
    }

    IEnumerator SwitchToJoinGame()
    {
        yield return new WaitForSeconds(10f);
        gameOverCanvas.SetActive(false);
        sceneSwitcher.SwitchScene("joinGame");
    }

}
