using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBall : BasicAutoAttack
{
    public Quaternion rotation;
    
    void FixedUpdate()
    {
        transform.position += transform.forward;
    }
}
