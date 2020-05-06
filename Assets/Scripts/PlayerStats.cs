using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float startHealth = 100f;
    public Image healthBar;

    public Text playerNumberText;
    public Text LifesText;
    public Text WeaponText;
    public Text AmmoText;

    public RectTransform statsContainer;

    public int lifesStart = 7;

    private float healthPoints;
    private int lifesLeft;

    private int playerNumber;
    private Color playerColor;

    public string weaponName;
    public float ammoLeft;

    //Death stuff
    public GameObject deathParticlesPrefab;
    public GameObject getDamageParticlesPrefab;
    public AudioClip deathSound;
    private GameObject getDamageParticlesGO;
    private ParticleSystem getDamageParticles;

    private AudioSource audioSource;

    //screen variables

    public float leftVar;
    public float topVar;
    public float rightVar;
    public float bottomVar;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        ResetPlayerStats();

        playerNumber = SpawnManager.instance.RequestPlayerNumber(gameObject);
        playerColor = SpawnManager.instance.RequestPlayerColor(playerNumber);

        UpdateStats();
        PlacementOfStats(playerNumber);

        getDamageParticlesGO = Instantiate(getDamageParticlesPrefab, transform.position, transform.rotation);
        getDamageParticlesGO.transform.SetParent(gameObject.transform);
        getDamageParticles = getDamageParticlesGO.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        PlacementOfStats(playerNumber);
    }

    public void DeathByFalling()
    {
        Die();
    }

    public void TakeDamage(float amount)
    {
        healthPoints -= amount;

        healthBar.fillAmount = healthPoints / startHealth;


        //play particle blood here
        getDamageParticles.Play();

        if (healthPoints <= 0)
        {
            Instantiate(deathParticlesPrefab, transform.position, transform.rotation);
            Die();

            audioSource.clip = deathSound;
            audioSource.Play();

        }
    }

    private void ResetHealth()
    {
        healthPoints = startHealth;
        healthBar.fillAmount = healthPoints / startHealth;
    }

    private void Die()
    {

        ResetHealth();
        GetComponent<PlayerController>().MoveToStartPosition();
        lifesLeft -= 1;


        if (lifesLeft <= 0)
        {
            GameManager.instance.GameOver();
        }

        UpdateStats();
    }

    public void ResetPlayerStats()
    {
        ResetHealth();
        lifesLeft = lifesStart;
        UpdateStats();
    }

    public void UpdateStats()
    {
        playerNumberText.text = (string)("Player " + playerNumber);
        playerNumberText.color = playerColor;
        LifesText.text = (string)("Lifes: " + lifesLeft);
        LifesText.color = playerColor;
        WeaponText.text = (string)("Weapon: " + weaponName);
        WeaponText.color = playerColor;
        AmmoText.text = (string)("Ammo: " + ammoLeft);
        AmmoText.color = playerColor;
    }

    private void PlacementOfStats(int playerNumber)
    {
        switch (playerNumber)
        {
            case 0:
                statsContainer.SetLeft(0);
                statsContainer.SetTop(0);
                statsContainer.SetRight(Screen.width -300);
                statsContainer.SetBottom(Screen.height - 300);
                return;

            case 1:
                statsContainer.SetLeft(Screen.width - 300);
                statsContainer.SetTop(0);
                statsContainer.SetRight(0);
                statsContainer.SetBottom(Screen.height - 300);
                return;

            case 2:
                statsContainer.SetLeft(0);
                statsContainer.SetTop(Screen.height - 300);
                statsContainer.SetRight(Screen.width - 300);
                statsContainer.SetBottom(0);
                return;

            case 3:
                statsContainer.SetLeft(Screen.width - 300);
                statsContainer.SetTop(Screen.height - 300);
                statsContainer.SetRight(0);
                statsContainer.SetBottom(0);
                return;
        }
        return;
    }



}
