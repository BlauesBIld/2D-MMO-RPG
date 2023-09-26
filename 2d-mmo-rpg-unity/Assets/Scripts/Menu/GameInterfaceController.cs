using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInterfaceController : MonoBehaviour
{
    public GameObject inGameInterface;

    void Start()
    {
        inGameInterface.SetActive(false);
    }
}
