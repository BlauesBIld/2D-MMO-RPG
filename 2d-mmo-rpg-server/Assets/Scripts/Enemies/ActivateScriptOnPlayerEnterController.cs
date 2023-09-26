using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateScriptOnPlayerEnterController : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<EnemySpawnerController>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            gameObject.GetComponent<EnemySpawnerController>().enabled = true;
        }
    }
}
