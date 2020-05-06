using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private Vector3 zeroPosition;
    [SerializeField] private Vector3 maxPostion;

    private Vector3 newPosition;

    [SerializeField]
    private List<GameObject> playerList = new List<GameObject>();

    //---Singleton-Pattern-----------------------
    public static SpawnManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SpawnManager in scene!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }
    //---Singleton-Pattern-ENDE------------------

    // Start is called before the first frame update
    void Start()
    {
        ResetBorderPositions();
    }

    public void ResetBorderPositions()
    {
        zeroPosition = GameObject.Find("BottomLeft").transform.position;
        maxPostion = GameObject.Find("TopRight").transform.position;

    }

    public void ResetAllPlayers()
    {
        foreach (GameObject player in playerList)
        {
            player.GetComponent<PlayerController>().ResetPlayer();
        }
    }

    public Vector3 GetNewPosition()
    {
        newPosition = new Vector3(Random.Range(zeroPosition.x, maxPostion.x), zeroPosition.y, Random.Range(zeroPosition.z, maxPostion.z));
        return newPosition;
    }

    public void AssignPlayer(GameObject player)
    {
        playerList.Add(player);

        //Player zu child machen um es in "DontDestroyOnLoad" des GameManagers einzubinden
        player.transform.SetParent(gameObject.transform);
    }

    public int RequestPlayerNumber(GameObject player)
    {
        int myNumber = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] == player)
            {
                myNumber = i;
            }
        }
        return myNumber;
    }

    public Color RequestPlayerColor(int playerNumber)
    {
        switch (playerNumber)
        {
            case 0:
                return Color.yellow;
            case 1:
                return Color.blue;
            case 2:
                return Color.green;
            case 3:
                return Color.magenta;
        }
        return Color.black;
    }

    public List<GameObject> RequestPlayerList()
    {
        return playerList;
    }
}