using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingManager : MonoBehaviour
{
    public static NetworkingManager instance;

    public GameObject playerPrefab;
    public GameObject parentObject;

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
        Application.targetFrameRate = 60;
        DAOGame.GetInstance().CreateAllTablesIfNotExisting();
        Server.Start(50, 13061);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        GameObject player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        player.transform.parent = parentObject.transform;
        return player.GetComponent<Player>();
    }
}