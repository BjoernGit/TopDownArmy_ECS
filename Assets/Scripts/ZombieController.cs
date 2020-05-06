using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class ZombieController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 targetPoint = new Vector3(0f, 0f, 0f);
    public List<GameObject> playerList;
    public Image healthBar;
    public GameObject canvasAnchor;
    public float startHealth;
    private float healthPoints;
    private Rigidbody rb;

    private ParticleSystem getDamageParticles;
    private AudioSource audioSource;
    public AudioClip deathSound;
    public GameObject getDamageParticlesPrefab;
    public GameObject deathParticlesPrefab;
    private GameObject getDamageParticlesGO;

    public GameObject[] splatterTexturePrefabs;
    public AudioClip[] gruntSounds;

    float gruntTimer = 0f;

    private float waitForTargetRecalculateIndex = 1f;
    private float distanceStep;

    public float navAgentTimer = 0f;

    private NavMeshPath path;
    private float elapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = startHealth;

        InvokeRepeating("SearchForTarget", 1f, 1f);
        audioSource = GetComponent<AudioSource>();

        getDamageParticlesGO = Instantiate(getDamageParticlesPrefab, transform.position, transform.rotation);
        getDamageParticlesGO.transform.SetParent(gameObject.transform);
        getDamageParticles = getDamageParticlesGO.GetComponent<ParticleSystem>();

        rb = GetComponent<Rigidbody>();

        path = new NavMeshPath();
        elapsed = 0.0f;
    }

    private void Update()
    {
        if (gruntTimer <= 0f)
        {
            //grunt
            audioSource.clip = gruntSounds[(int)Random.Range(0f, gruntSounds.Length)];
            audioSource.Play();
            gruntTimer = 10f + gruntSounds.Length;
        }
        else
        {
            //reduce timer
            gruntTimer -= Time.deltaTime;
        }


        // Update the way to the goal every second.
        //elapsed += Time.deltaTime;
        //if (elapsed > 1.0f)
        //{
        //    elapsed -= 1.0f;
        //    //NavMesh.CalculatePath(transform.position, agent.destination, NavMesh.AllAreas, path);
        //}
        //for (int i = 0; i < path.corners.Length - 1; i++)
        //    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);


    }

    private void SearchForTarget()
    {


        playerList = SpawnManager.instance.RequestPlayerList();
        float shortestDistanceToPlayer = Mathf.Infinity;

        foreach (GameObject player in playerList)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= shortestDistanceToPlayer)
            {
                shortestDistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                targetPoint = player.transform.position;
            }
        }

        distanceStep = 100 / (shortestDistanceToPlayer * shortestDistanceToPlayer);
        waitForTargetRecalculateIndex += distanceStep;

        if (waitForTargetRecalculateIndex > 1f || agent.remainingDistance <= 0.1f)
        {
            if (agent.enabled == true)
            {

                agent.SetDestination(targetPoint);
                waitForTargetRecalculateIndex = 0f;

            }
        }

    }

    public void TakeDamage(float amount)
    {
        healthPoints -= amount;

        canvasAnchor.SetActive(true);
        healthBar.fillAmount = healthPoints / startHealth;

        SpawnBloodTexture();

        //play particle blood here
        getDamageParticles.Play();

        if (healthPoints <= 0)
        {
            Instantiate(deathParticlesPrefab, transform.position, transform.rotation);

            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);

        SoundObjectManager.instance.CreateSoundObjectForSeconds(transform.position, deathSound);

    }

    private void SpawnBloodTexture()
    {
        //Calculate Vector 3 for position
        Vector3 bloodPosition = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z);
        Quaternion bloodRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        //choose random texture prefab
        GameObject chosenTexture = splatterTexturePrefabs[(int)Random.Range(0f, splatterTexturePrefabs.Length)];

        //instantiate texture prefab
        GameObject bloodGO = Instantiate(chosenTexture, bloodPosition, bloodRotation);

        //resize texture to half the size
        bloodGO.transform.localScale *= 0.5f;

        //add BloodObject to meshbaker
        BakeBloodManager.instance.AddObjectToBakePool(bloodGO);

    }



}
