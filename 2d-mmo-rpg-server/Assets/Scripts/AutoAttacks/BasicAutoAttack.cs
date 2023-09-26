using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAutoAttack : MonoBehaviour
{
    [NonSerialized]
    public string name;
    
    public float defaultSpeed;
    public float defaultRange;

    public float magicDamage;
    public float physicalDamage;

    public Vector3 startPosition;
    
    void Start()
    {
        name = gameObject.name;
    }
}
