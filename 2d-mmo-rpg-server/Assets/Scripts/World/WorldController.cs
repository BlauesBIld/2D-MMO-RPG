using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static Player[] GetPlayersWithinRadius(Vector3 position, float radius)
    {
        Collider[] objectsInRadius = Physics.OverlapSphere(position, radius);
        List<Player> playersInRadius = new List<Player>();
        for (int i = 0; i < objectsInRadius.Length; i++)
        {
            if (objectsInRadius[i].gameObject.GetComponent<Player>() != null)
            {
                playersInRadius.Add(objectsInRadius[i].GetComponentInParent<Player>());
            }
        }
        return playersInRadius.ToArray();
    }
}
