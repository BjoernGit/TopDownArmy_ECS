using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttacks : MonoBehaviour
{
    public float damage = 10f;
    private float timer = 0f;
    public float timeBetweenHits = 3f;
    public float attackPowerMultiplier;

    public AudioClip attackSound;

    private void OnTriggerStay(Collider other)
    {
        if (timer <= 0)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
                other.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * attackPowerMultiplier);
                SoundObjectManager.instance.CreateSoundObjectForSeconds(gameObject.transform.position, attackSound);
            }
            else if (other.gameObject.GetComponent<Rigidbody>() != null)
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * attackPowerMultiplier);

            }
            timer = timeBetweenHits;
        }
        else
        {
            timer -= Time.deltaTime;
        }

    }
}
