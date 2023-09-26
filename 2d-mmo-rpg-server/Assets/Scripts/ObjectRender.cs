using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRender : MonoBehaviour
{
    public static List<Player> GetNearbyPlayersAndEnemies(Transform _transform, float _radius)
    {
        List<Player> _list = new List<Player>();
        Collider[] objectsInRadius = Physics.OverlapSphere(_transform.position, _radius);
        for (int i = 0; i < objectsInRadius.Length; i++)
        {
            if ((objectsInRadius[i].gameObject.GetComponent<Player>() != null))
            {
                _list.Add(objectsInRadius[i].gameObject.GetComponent<Player>());
            }
        }
        return _list;
    }

    public static void UpdateNearbyPlayerList(List<Player> _nearbyList, Transform _transform, float _radius)
    {
        List<Player> _newList = GetNearbyPlayersAndEnemies(_transform, _radius);
        bool _newPlayersInRadius = !new HashSet<Player>(_nearbyList).SetEquals(_newList);
        if (_newPlayersInRadius)
        {
            // loop to add new players
            for (int i = 0; i < _newList.Count; i++)
            {
                if (!_nearbyList.Contains(_newList[i]))
                {
                    _nearbyList.Add(_nearbyList[i]);
                }
            }
            // loop to remove old players
            for (int i = 0; i < _nearbyList.Count; i++)
            {
                if (!_newList.Contains(_nearbyList[i]))
                {
                    _nearbyList.Remove(_nearbyList[i]);
                }
            }
        }
    }

    // Old function
    /* 
    public bool CheckForNewObjectsInRadius(List<GameObject> _nearbyList, Transform _transform, float _radius)
    {
        List<GameObject> _newList = GetNearbyPlayersAndEnemies(_transform, _radius);
        bool a = new HashSet<GameObject>(_nearbyList).SetEquals(_newList);
        return !a;
    }
    */

}
