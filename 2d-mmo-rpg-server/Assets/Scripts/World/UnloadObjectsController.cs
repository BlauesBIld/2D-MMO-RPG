using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadObjectsController : MonoBehaviour
{
    private void OnTriggerExit(Collider exitingCollider)
    {
        if (exitingCollider.gameObject.GetComponent<Enemy>() != null)
        {
            if (GetComponentInParent<Player>().enemiesNearby.Contains(exitingCollider.GetComponent<Enemy>()))
            {
                exitingCollider.gameObject.GetComponent<Enemy>().playersNearby.Remove(GetComponentInParent<Player>());
                GetComponentInParent<Player>().enemiesNearby.Remove(exitingCollider.GetComponent<Enemy>());
                ServerSend.RemoveEnemy(GetComponentInParent<Player>().id,
                    exitingCollider.gameObject.GetComponent<Enemy>());
            }
        }
        else if (exitingCollider.gameObject.GetComponent<Player>() != null && exitingCollider.gameObject != gameObject)
        {
            if (GetComponentInParent<Player>().playersNearby.Contains(exitingCollider.GetComponent<Player>()))
            {
                exitingCollider.gameObject.GetComponent<Player>().playersNearby.Remove(GetComponentInParent<Player>());
                GetComponentInParent<Player>().playersNearby.Remove(exitingCollider.GetComponent<Player>());
                ServerSend.RemovePlayer(GetComponentInParent<Player>().id,
                    exitingCollider.gameObject.GetComponent<Player>());
            }
        }
        else
        {
            if (GetComponentInParent<Player>().environmentObjectsNearby.Contains(exitingCollider.gameObject))
            {
                GetComponentInParent<Player>().environmentObjectsNearby.Remove(exitingCollider.gameObject);
                ServerSend.RemoveEnvironmentObj(GetComponentInParent<Player>(), exitingCollider.gameObject.name,
                    exitingCollider.gameObject.transform.position);
            }
        }
    }
}