using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public Transform[] wayPoints;
    public int enemiesPerWave = 5;
    private int currentEnemiesAlive = 0;
    public float timeBetweenWaves = 5f;
    public float spawnDelay = 0.5f;

    private int currentWave = 1;
    private bool isSpawning = false;

    void Update()
    {
        //Start a new wave if not currently spawning and no enemies are alive
        if (!isSpawning && currentEnemiesAlive == 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;
        Debug.Log($"Starting wave {currentWave} with {enemiesPerWave} enemies.");

        //Spawn all enemies for the current wave
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay); 
        }

        //Wait before the next wave
        yield return new WaitForSeconds(timeBetweenWaves);

        //Prepare for the next wave
        currentWave++;
        //Add more enemies per wave
        enemiesPerWave += 2; 
        isSpawning = false;
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        //Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Instantiate the enemy at the chosen spawn point
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        //Assign waypoints to the enemy's script
        WayPoints enemyScript = enemy.GetComponent<WayPoints>();
        if (enemyScript != null)
        {
            enemyScript.wayPoints = wayPoints;
        }
        else
        {
            Debug.LogWarning("Spawned enemy does not have a WayPoints script.");
        }

        //Track the number of alive enemies
        currentEnemiesAlive++;

        //Attach event listener for death handling
        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            //Subscribe to the OnDeath event
            enemyHealth.OnDeath += HandleEnemyDeath; 
        }
        else
        {
            Debug.LogWarning("Spawned enemy does not have a Health component.");
        }
    }

    private void HandleEnemyDeath()
    {
        currentEnemiesAlive--;
        Debug.Log($"Enemy died. {currentEnemiesAlive} remaining.");
    }
}
