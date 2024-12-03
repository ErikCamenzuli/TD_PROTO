using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public Transform[] wayPoints;
    public int enemiesPerwave = 5;
    public float timeBetweenWaves = 5f;
    public float spawnDelay = 0.5f;

    private int currentWave = 1;
    private bool isSpawning = false;

    // Update is called once per frame
    void Update()
    {
        //Checks to see if there's a wave spawnning, if not then the wave will start
        if(!isSpawning)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        //Activate the spawning of the wave
        isSpawning = true;

        //Looping how many enemies per wave there is  
        for(int i = 0; i < enemiesPerwave; i++)
        {
            //Spawns enemy
            SpawnEnemy();
            //Waiting between each spawn
            yield return new WaitForSeconds(spawnDelay);
        }

        //Waiting between each wave
        yield return new WaitForSeconds(timeBetweenWaves);
        //Increment current wave by 1
        currentWave++;
        //Adding 2 each enemies on each new wave
        enemiesPerwave += 2;
        //Disabling the spawning on the wave
        isSpawning = false;

    }

    private void SpawnEnemy()
    {
        //Setting the array up for the position of the spawn point(s)
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Setting the enemys gameobject to the prefab, the position of the spawn point and setting no rotation to the prefab
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        //Setting enemy script to the enemys component for waypoints
        WayPoints enemyScript = enemy.GetComponent<WayPoints>();

        //Checking to see if the enemies script is null then setting it to
        //the waypoints
        if(enemyScript != null)
        {
            enemyScript.wayPoints = wayPoints;
        }
    }

}
