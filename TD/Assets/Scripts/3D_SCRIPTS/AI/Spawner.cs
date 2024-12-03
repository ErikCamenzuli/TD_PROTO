using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public Transform[] wayPoints;
    public int enemiesPerwave = 5;
    public float timeBetweenWabes = 5f;
    public float spawnDelay = 0.5f;

    private int currentWave = 1;
    private bool isSpawning = false;

    // Update is called once per frame
    void Update()
    {
        if(!isSpawning)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;

        for(int i = 0; i < enemiesPerwave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(timeBetweenWabes);
        currentWave++;
        enemiesPerwave += 2;
        isSpawning = false;

    }

    private void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        WayPoints enemyScript = enemy.GetComponent<WayPoints>();

        if(enemyScript != null)
        {
            enemyScript.wayPoints = wayPoints;
        }
    }

}
