using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObjectManager : MonoBehaviour
{

    //---Singleton-Pattern-----------------------
    public static SoundObjectManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SoundObjectManager in scene!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }
    //---Singleton-Pattern-ENDE------------------

    public void CreateSoundObjectForSeconds(Vector3 position, AudioClip audioClip)
    {
        GameObject GO = new GameObject("GO");
        
        GO.AddComponent<AudioSource>();

        GO.GetComponent<AudioSource>().clip = audioClip;
        GO.GetComponent<AudioSource>().volume = 0.3f;
        GO.GetComponent<AudioSource>().Play();
        Destroy(GO, audioClip.length);
    }

}
