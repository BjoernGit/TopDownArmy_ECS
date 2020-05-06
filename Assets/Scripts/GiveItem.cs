using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GiveItem : MonoBehaviour
{
    public string itemName;

    public AudioClip pickUpClip;

    public GameObject spawnPositionMaster;


    private void Start()
    {
        //spawnPositionMaster = GameObject.Find("ItemSpawnPosition");
        spawnPositionMaster = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //deactiviere item
            spawnPositionMaster.GetComponent<ItemSpawnPosition>().DeactivateItem(itemName, gameObject, other.gameObject, pickUpClip);


        }

    }

}
