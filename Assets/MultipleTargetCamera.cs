using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{

    public List<Transform> targets;
    private List<GameObject> playerObjects;
    private int lastPlayerCount = 0;

    public Vector3 offset;

    private void LateUpdate()
    {
        //this initialisation should be in the start method but since I am testing a lot the player do join during
        //the startingperiod of this level. in the end version this would not be the case.

        playerObjects = SpawnManager.instance.RequestPlayerList();
        if (playerObjects.Count == 0)
        {
            return;
        }

        if (lastPlayerCount < playerObjects.Count)
        {
            foreach (GameObject playerObject in playerObjects)
            {
                if (!targets.Contains(playerObject.transform))
                {
                    targets.Add(playerObject.transform);
                }
            }
            lastPlayerCount = playerObjects.Count;
        }

        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = newPosition;


    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }


}
