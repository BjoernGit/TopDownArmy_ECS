using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPosition : MonoBehaviour
{
    public GameObject[] optionsToSpawn;

    private AudioSource audioSource;

    public GameObject itemAtPosition;

    public float timerToSpawn = 5f;

    public float timerMax = 5f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //if no item start timer to spawn
        if (itemAtPosition == null)
        {
            timerToSpawn -= Time.deltaTime;
        }

        if (timerToSpawn <= 0f)
        {
            SpawnItem();
            timerToSpawn = timerMax;
        }
    }

    public void SpawnItem()
    {
        //spawn Item from Options
        int rand = (int)Random.Range(0f, optionsToSpawn.Length);
        itemAtPosition = Instantiate(optionsToSpawn[rand]);
        itemAtPosition.transform.position = gameObject.transform.position;
        itemAtPosition.transform.SetParent(gameObject.transform);
    }

    public void DeactivateItem(string itemName, GameObject itemToDeactivate, GameObject player, AudioClip pickUpClip)
    {
        //Funktion in Player auslösen
        player.SendMessage("OnItemPickup", itemName);

        //object deactivieren
        Destroy(itemToDeactivate);

        //sound auslösen
        audioSource.clip = pickUpClip;
        audioSource.Play();
    }

}
