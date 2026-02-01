using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs;

    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private float timeBetweenSpawnsScalingAmount = .9f;
    [SerializeField] private float timeBetweenScalingEvents = 15f;

    [SerializeField] private float minimumSpawnRange;
    [SerializeField] private float maximumSpawnRange;

    private float timeOfLevelStart;
    private float scalingEventCount = 0f;

    private float timeOfLastSpawn = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeOfLevelStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - timeOfLevelStart) / timeBetweenScalingEvents > scalingEventCount)
        {
            scalingEventCount += 1;
            timeBetweenSpawns *= timeBetweenSpawnsScalingAmount;
        }
        if(Time.time - timeOfLastSpawn > timeBetweenSpawns)
        {
            timeOfLastSpawn = Time.time;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        float randX = UnityEngine.Random.Range(minimumSpawnRange, maximumSpawnRange);
        int flip = UnityEngine.Random.Range(0, 2);
        if (flip == 1)
        {
            randX *= -1f;
        }
        float randY = UnityEngine.Random.Range(minimumSpawnRange, maximumSpawnRange);
        flip = UnityEngine.Random.Range(0, 2);
        if (flip == 1)
        {
            randY *= -1f;
        }
        int enemyIndexToSpawn = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        Vector3 offset = new Vector3(randX, randY, 0f);
        GameObject.Instantiate(enemyPrefabs[enemyIndexToSpawn], transform.position + offset, Quaternion.identity);
    }
}
