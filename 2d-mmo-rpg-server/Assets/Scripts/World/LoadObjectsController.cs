using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadObjectsController : MonoBehaviour
{
    private void OnTriggerEnter(Collider enteringCollider)
    {
        if (enteringCollider.gameObject.GetComponent<Enemy>() != null)
        {
            if (!GetComponentInParent<Player>().enemiesNearby.Contains(enteringCollider.GetComponent<Enemy>()))
            {
                enteringCollider.gameObject.GetComponent<Enemy>().playersNearby.Add(GetComponentInParent<Player>());
                GetComponentInParent<Player>().enemiesNearby.Add(enteringCollider.GetComponent<Enemy>());
                ServerSend.SpawnEnemy(GetComponentInParent<Player>().id,
                    enteringCollider.gameObject.GetComponent<Enemy>());
            }
        }
        /*else if (enteringCollider.gameObject != gameObject && enteringCollider.gameObject.GetComponent<Player>() != null)
        {
            if (!GetComponentInParent<Player>().playersNearby.Contains(enteringCollider.GetComponent<Player>()))
            {
                enteringCollider.gameObject.GetComponent<Player>().playersNearby.Add(GetComponentInParent<Player>());
                GetComponentInParent<Player>().playersNearby.Add(enteringCollider.GetComponent<Player>());
                ServerSend.SpawnPlayer(GetComponentInParent<Player>().id,
                    enteringCollider.gameObject.GetComponent<Player>());
            }
        }*/
        else
        {
            if (!GetComponentInParent<Player>().environmentObjectsNearby.Contains(enteringCollider.gameObject))
            {
                GetComponentInParent<Player>().environmentObjectsNearby.Add(enteringCollider.gameObject);
                ServerSend.SpawnEnvironmentObj(GetComponentInParent<Player>(), enteringCollider.gameObject.name,
                    enteringCollider.gameObject.transform.position);
            }
        }
    }
}