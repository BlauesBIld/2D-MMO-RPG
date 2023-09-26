using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, EnemyBaseController> enemies = new Dictionary<int, EnemyBaseController>();

    public static Dictionary<string, GameObject> enemyDictionary = new Dictionary<string, GameObject>();
    public List<GameObject> autoAttackPrefabs = new List<GameObject>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public GameObject playerParent;
    public GameObject enemyParent;
    public GameObject environmentParent;

    public Dictionary<String, GameObject> environmentObjectsNearby = new Dictionary<string, GameObject>();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        object[] tempAutoAttackPrefabs = Resources.LoadAll("Prefabs/AutoAttacks", typeof(GameObject));

        foreach (GameObject autoAttackPrefab in tempAutoAttackPrefabs)
        {
            autoAttackPrefabs.Add(autoAttackPrefab);
        }

        Debug.Log("Initialized AutoAttack Prefabs");
        
        InitializeEnemyDictionary();
    }

    public void SpawnPlayer(int id, string username, Vector3 position)
    {
        GameObject player;
        if (id == Client.instance.myId)
        {
            player = Instantiate(localPlayerPrefab, position, Quaternion.Euler(80, 0, 0));
        }
        else
        {
            player = Instantiate(playerPrefab, position, Quaternion.Euler(80, 0, 0));
        }
        
        player.GetComponent<PlayerManager>().Initialize(id, username);
        player.transform.parent = playerParent.transform;

        players.Add(id, player.GetComponent<PlayerManager>());
    }

    public void SpawnEnemy(int id, Vector3 position, float maxHealth, float currentHealth, string type)
    {
        // if - else just as test because only Bomber and Dummy prefab exists 
        // TODO: Delete Later
        if (type != "Bomber")
        {
            GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.Euler(80, 0, 0));

            enemy.GetComponent<EnemyBaseController>().Initialize(id, maxHealth, currentHealth);
            enemy.transform.parent = enemyParent.transform;
            enemies.Add(id, enemy.GetComponent<EnemyBaseController>());
        }
        else
        {
            GameObject prefab = enemyDictionary[type];
            GameObject enemy = Instantiate(prefab, position, Quaternion.Euler(80, 0, 0));

            enemy.GetComponent<EnemyBaseController>().Initialize(id, maxHealth, currentHealth);
            enemy.transform.parent = enemyParent.transform;
            enemies.Add(id, enemy.GetComponent<EnemyBaseController>());
        }
    }

    public void RemoveEnemy(int id)
    {
        enemies[id].Die();
        enemies.Remove(id);
    }

    public void SpawnEnvironmentObj(string assetName, Vector3 transformPosition)
    {
        GameObject spawningEnvironmentObject = new GameObject();
        SetParametersForSpawningEnvironmentObj(spawningEnvironmentObject, assetName, transformPosition);
        environmentObjectsNearby.Add(assetName + transformPosition, spawningEnvironmentObject);
    }

    private void SetParametersForSpawningEnvironmentObj(GameObject spawningEnvironmentObject, string assetName, Vector3 transformPosition)
    {
        spawningEnvironmentObject.name = assetName;
        spawningEnvironmentObject.AddComponent<SpriteRenderer>();
        spawningEnvironmentObject.GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Environment/Grassland/" + assetName);
        spawningEnvironmentObject.transform.position = transformPosition;
        spawningEnvironmentObject.transform.rotation = Quaternion.Euler(80, 0, 0);
        spawningEnvironmentObject.transform.parent = environmentParent.transform;
    }

    public void RemoveEnvironmentObj(string assetName, Vector3 transformPosition)
    {
        Destroy(environmentObjectsNearby[assetName + transformPosition]);
        environmentObjectsNearby.Remove(assetName + transformPosition);
    }

    public void InitializeEnemyDictionary()
    {
        object[] tempEnemyPrefabs = Resources.LoadAll("Prefabs/Enemies", typeof(GameObject));
        
        foreach (GameObject prefabEnemy in tempEnemyPrefabs)
        {
            enemyDictionary.Add(prefabEnemy.name, prefabEnemy);
        }

        Debug.Log("Initialized Enemy Prefabs");
    }
}
