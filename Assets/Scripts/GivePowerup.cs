using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePowerup : MonoBehaviour
{
    [Header("Selection of PowerUp")]
    public bool increasePower;
    public bool increaseJumps;
    public bool gravityGun;
    public bool increaseMass;
    public bool increaseShootSpeed;

    [Header("Duration Settings")]
    public float coolDownOfPowerUp = 10f;
    public float multiplierDuration = 5f;

    [Header("Multiplier Settings")]
    public float multiplierSize = 2f;

    [Header("Sound Files")]
    public AudioClip increasePowerClip;
    public AudioClip increaseJumpsClip;
    public AudioClip gravityGunClip;
    public AudioClip increaseMassClip;
    public AudioClip increaseShootSpeedClip;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (increasePower)
            {
                other.gameObject.GetComponent<PlayerController>().PowerUpIncreasePower(multiplierDuration, multiplierSize);
                audioSource.clip = increasePowerClip;
                audioSource.Play();
            }

            if (increaseJumps)
            {
                other.gameObject.GetComponent<PlayerController>().PowerUpIncreaseJumps(multiplierDuration, multiplierSize);
                audioSource.clip = increaseJumpsClip;
                audioSource.Play();
            }

            if (gravityGun)
            {
                other.gameObject.GetComponent<PlayerController>().PowerUpGravityGun(multiplierDuration, multiplierSize);
                audioSource.clip = gravityGunClip;
                audioSource.Play();
            }

            if (increaseMass)
            {
                other.gameObject.GetComponent<PlayerController>().PowerUpIncreaseMass(multiplierDuration, multiplierSize);
                audioSource.clip = increaseMassClip;
                audioSource.Play();
            }

            if (increaseShootSpeed)
            {
                other.gameObject.GetComponent<PlayerController>().PowerUpShootSpeed(multiplierDuration, multiplierSize);
                audioSource.clip = increaseShootSpeedClip;
                audioSource.Play();
            }

            StartCoroutine(ActivateAfterTime(coolDownOfPowerUp));
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    IEnumerator ActivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
    }
}
