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
        Debug.Log($"Update called. EnemiesAlive: {currentEnemiesAlive}, IsSpawning: {isSpawning}");
        if (!isSpawning && currentEnemiesAlive == 0)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;
        Debug.Log($"Starting wave {currentWave} with {enemiesPerWave} enemies.");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(timeBetweenWaves);

        currentWave++;
        enemiesPerWave += 2;
        isSpawning = false;

        Debug.Log($"Wave {currentWave} prepared. Next wave will have {enemiesPerWave} enemies.");
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

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"Enemy spawned at {spawnPoint.position}");

        WayPoints enemyScript = enemy.GetComponent<WayPoints>();
        if (enemyScript != null)
        {
            enemyScript.wayPoints = wayPoints;
        }
        else
        {
            Debug.LogWarning("Spawned enemy does not have a WayPoints script.");
        }

        currentEnemiesAlive++;
        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
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
        Debug.Log($"Enemy died. Remaining enemies: {currentEnemiesAlive}");
    }
}
