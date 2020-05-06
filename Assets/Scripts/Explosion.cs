using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Explosion : MonoBehaviour
{
    public float radius = 10.0f;
    public float power = 10.0f;
    public float damageMultiplier = 1f;
    public float timeToKillSelf = 5f;
    public bool deactivateGravity = false;
    public GameObject playerWhoShot;
    public AudioClip collisionSound;
    public bool coopModeActive = false;
    public GameObject CollisionFXPrefab;

    private void Awake()
    {
        StartCoroutine("KillSelf", timeToKillSelf);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CastExplosion();
    }

    private void CastExplosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (deactivateGravity)
                {
                    ReduceGravity _rg = hit.gameObject.GetComponent<ReduceGravity>();
                    if (_rg != null)
                    {
                        _rg.ResetGravityTimer();
                    }
                    else
                    {
                        ReduceGravity rg = hit.gameObject.AddComponent<ReduceGravity>() as ReduceGravity;
                    }
                }

                if (!hit.gameObject.CompareTag("Enemy"))
                {
                    rb.AddExplosionForce(power, explosionPos, radius, 0.0f);

                }

                if (hit.gameObject.CompareTag("Player") && (hit.gameObject != playerWhoShot || coopModeActive))
                {
                    //kehrwert von der distance der hit position zur explosions position      
                    hit.gameObject.GetComponent<PlayerStats>().TakeDamage(damageMultiplier * (1 / (Vector3.Distance(hit.gameObject.transform.position, explosionPos))));
                }
                

            }
            else if (hit.gameObject.CompareTag("Enemy"))
            {

                hit.gameObject.GetComponent<ZombieController>().TakeDamage(damageMultiplier * (1 / (Vector3.Distance(hit.gameObject.transform.position, explosionPos))));

            }

        }
        if (playerWhoShot.name == "BombDropper")
        {
            playerWhoShot.GetComponent<AudioSource>().clip = collisionSound;
            playerWhoShot.GetComponent<AudioSource>().Play();
        }

        Instantiate(CollisionFXPrefab, gameObject.transform.position, Quaternion.Euler(Vector3.up));

        Destroy(gameObject);
    }

    private IEnumerator KillSelf()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

}
