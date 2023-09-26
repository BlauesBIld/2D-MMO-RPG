using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerController : MonoBehaviour
{
    public int counterTotalEnemies = 0;
    
    public int currentAmountOfEnemiesAtATime = 0;
    public int maxTotalEnemies = 15;
    public int maxEnemiesAtATime = 5;
    public GameObject enemyPrefab;
    public int radius = 10;
    public int spawnDelayInSeconds = 5;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < maxEnemiesAtATime; i++)
        {
            AddEnemy();
        }

        StartCoroutine(SpawnEnemyIfNotMax());
    }

    private IEnumerator SpawnEnemyIfNotMax()
    {
        while (counterTotalEnemies < maxTotalEnemies)
        {
            if (currentAmountOfEnemiesAtATime < maxEnemiesAtATime)
            {
                for (int i = currentAmountOfEnemiesAtATime; i < maxEnemiesAtATime; i++)
                {
                    AddEnemy();
                    yield return new WaitForSeconds(spawnDelayInSeconds);
                }
            }

            yield return new WaitForSeconds(3f);
        }

        while (true)
        {
            if (currentAmountOfEnemiesAtATime <= 0)
            {
                Destroy(gameObject);
            }

            yield return new WaitForSeconds(5f);
        }
    }

    public void AddEnemy()
    {
        counterTotalEnemies++;
        currentAmountOfEnemiesAtATime++;
        Enemy newEnemy = Instantiate(enemyPrefab, transform.position + GetRandomPositionWithinRadius(radius), Quaternion.identity).GetComponent<Enemy>();
        newEnemy.transform.parent = gameObject.transform;
        EnemyManager.instance.AddEnemy(newEnemy);
    }

    private Vector3 GetRandomPositionWithinRadius(int radius)
    {
        return new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
    }
}