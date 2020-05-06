using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBomb : MonoBehaviour
{
    public GameObject bombPrefab;
    public AudioClip bombSoundClip;
    private AudioSource audioSource;

    public GameObject targetIndicator;

    public LayerMask layerMask;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("DropBombGO",5f,10f);
    }

    private void DropBombGO()
    {
        Vector3 bombDropLocation = SpawnManager.instance.GetNewPosition();

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(bombDropLocation, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity , layerMask))
        {
            GameObject TI = Instantiate(targetIndicator, hit.point, hit.transform.rotation);
            Destroy(TI, 5f);
        }

        GameObject bombGO = Instantiate(bombPrefab, bombDropLocation + 100f * Vector3.up, transform.rotation);
        bombGO.GetComponent<Explosion>().playerWhoShot = this.gameObject;

        audioSource.clip = bombSoundClip;
        audioSource.Play();
    }
}
