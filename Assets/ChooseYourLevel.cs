using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChooseYourLevel : MonoBehaviour
{
    private List<GameObject> playerList = new List<GameObject>();
    public LayerMask layerMask;

    [Header("SelectionLevelVariables")]
    Vector2 selection_movement;
    Vector2 selectionPosition = new Vector2(0f, 0f);
    float threshold = 0.8f;
    float stepSize = 2f;
    bool selectionMoved = false;
    public LayerMask levelSelectLayerMask;
    
    // Update is called once per frame
    void Update()
    {
        playerList = SpawnManager.instance.RequestPlayerList();
        foreach (GameObject player in playerList)
        {
            player.GetComponent<PlayerController>().levelSelectControls = true;
            player.GetComponent<PlayerController>().levelSelectLayerMask = layerMask;

        }
    }

    private void OnDestroy()
    {
        foreach (GameObject player in playerList)
        {
            player.GetComponent<PlayerController>().levelSelectControls = false;
            player.GetComponent<PlayerController>().levelSelectLayerMask = layerMask;

        }
    }

    public void MoveSelecter(InputValue value)
    {
        selection_movement = value.Get<Vector2>();
        if (selection_movement.x <= -threshold && !selectionMoved)
        {
            //move left
            GameObject.Find("SelectArea").transform.GetChild(0).Translate(-stepSize, 0f, 0f);
            selectionPosition.x -= stepSize;
            CheckSelectionPosition();
            selectionMoved = true;
        }
        if (selection_movement.x >= threshold && !selectionMoved)
        {
            //move right
            GameObject.Find("SelectArea").transform.GetChild(0).Translate(stepSize, 0f, 0f);
            selectionPosition.x += stepSize;
            CheckSelectionPosition();
            selectionMoved = true;
        }
        if (selection_movement.y <= -threshold && !selectionMoved)
        {
            //move down
            GameObject.Find("SelectArea").transform.GetChild(0).Translate(0f, -stepSize, 0f);
            selectionPosition.y -= stepSize;
            CheckSelectionPosition();
            selectionMoved = true;
        }
        if (selection_movement.y >= threshold && !selectionMoved)
        {
            //move up
            GameObject.Find("SelectArea").transform.GetChild(0).Translate(0f, stepSize, 0f);
            selectionPosition.y += stepSize;
            CheckSelectionPosition();
            selectionMoved = true;
        }
        //reset of selection movement
        if (selection_movement.magnitude < threshold)
        {
            //reset
            selectionMoved = false;
        }
    }

    private void CheckSelectionPosition()
    {
        //überprüfe ob der rahmen noch im sichtaberen bereich ist. ansonsten teleportiere
        //den Rahmen an die ander Seite des Rahmenbereiches.
        Transform selecterTransform = GameObject.Find("SelectArea").transform.GetChild(0);

        if (selectionPosition.x <= -2f * stepSize)
        {
            selectionPosition.x += 3f * stepSize;
            selecterTransform.transform.Translate(3f * stepSize, 0f, 0f);
        }
        if (selectionPosition.x >= 2f * stepSize)
        {
            selectionPosition.x -= 3f * stepSize;
            selecterTransform.transform.Translate(-3f * stepSize, 0f, 0f);
        }


        if (selectionPosition.y <= -2f * stepSize)
        {
            selectionPosition.y += 3f * stepSize;
            selecterTransform.transform.Translate(0f, 3f * stepSize, 0f);
        }
        if (selectionPosition.y >= 2f * stepSize)
        {
            selectionPosition.y -= 3f * stepSize;
            selecterTransform.transform.Translate(0f, -3f * stepSize, 0f);
        }
    }
}
