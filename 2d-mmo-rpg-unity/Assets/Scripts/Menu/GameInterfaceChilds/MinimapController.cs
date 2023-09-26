using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UIElements.Image;

public class MinimapController : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, 600, player.transform.position.z);
        }
        else
        {
            InstantiateMinimap();
        }

    }

    public void InstantiateMinimap()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
