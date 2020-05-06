using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieLevelController : MonoBehaviour
{
    public RectTransform statsContainer;
    public Text waveNumberText;
    public Text waveCountdownText;
    public GameObject zombiePrefab;
    public int zombieIncreaseEachWave;

    [SerializeField] private float countdown = 10f;
    [SerializeField] private int waveIndex = 5;
    [SerializeField] private float timeBetweenWaves = 60f;

    public List<GameObject> spawnPoints = new List<GameObject>();
    private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);


        //-------------------------
        PlacementOfStats();
    }

    IEnumerator SpawnWave()
    {
        waveIndex += 1;

        for (int i = 0; i < waveIndex; i++)
        {
            for (int j = 0; j < zombieIncreaseEachWave; j++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.2f);

            }
        }

        waveNumberText.text = "Wave: " + string.Format("{00}", waveIndex);
    }

    void SpawnEnemy()
    {
        spawnPoint = spawnPoints[(int)Random.Range(0f, spawnPoints.Count)].transform.position;
        Instantiate(zombiePrefab, spawnPoint, Quaternion.identity);
    }

    private void PlacementOfStats()
    {
        statsContainer.SetLeft(0);
        statsContainer.SetTop(0);
        statsContainer.SetRight(0);
        statsContainer.SetBottom(Screen.height - 300);
    }
}