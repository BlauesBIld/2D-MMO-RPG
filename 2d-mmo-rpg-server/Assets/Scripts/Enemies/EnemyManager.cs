using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public GameObject parentObject;

    public Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();
    public Dictionary<int, EnemySpawnerController> enemySpawners = new Dictionary<int, EnemySpawnerController>();

    public int maxEnemySpawner;
    public List<GameObject> specificEnemySpawnerPrefabs = new List<GameObject>();

    public static EnemyManager instance = new EnemyManager();
    
    private EnemyManager()
    {
    }

    public void Start()
    {
        foreach (GameObject enemySpawner in specificEnemySpawnerPrefabs)
        {
            for (int i = 0; i < Random.Range(50,100); i++)
            {
                GameObject spawnedSpawner = Instantiate(enemySpawner, GetRandomPositionWithinRadius(400), Quaternion.identity);
                spawnedSpawner.transform.parent = parentObject.transform;
            }
        }
    }

    public void AddEnemy(Enemy newEnemy)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                newEnemy.Initialize(i, 0, 0);
                enemies[i] = newEnemy;
                newEnemy.gameObject.name = newEnemy.GetType().Name + "_" + i;
                return;
            }
        }

        newEnemy.Initialize(enemies.Count, 0, 0);
        newEnemy.gameObject.name = newEnemy.GetType().Name + "_" + enemies.Count;
        enemies.Add(enemies.Count, newEnemy);
    }

    private Vector3 GetRandomPositionWithinRadius(int radius)
    {
        return new Vector3(Random.Range(-radius, radius), 1, Random.Range(-radius, radius));
    }
}