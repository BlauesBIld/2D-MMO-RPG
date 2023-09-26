using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentGeneration : MonoBehaviour
{
    public GameObject parentObject;

    private float worldMaxX = 100;
    private float worldMaxY = 100;

    private Dictionary<String, List<Vector3>> environmentObjectLocations = new Dictionary<string, List<Vector3>>();

    private void Start()
    {
        GenerateTreeLocs();
        GenerateGreeneryLocs();
        foreach (KeyValuePair<String, List<Vector3>> entry in environmentObjectLocations)
        {
            foreach (Vector3 location in entry.Value)
            {
                InstantiateTree(entry.Key, location);
            }
        }
    }

    private void InstantiateTree(String name, Vector3 location)
    {
        GameObject tree = new GameObject(name);
        SetParametersForInstantiatingTree(tree, location);
    }

    private void SetParametersForInstantiatingTree(GameObject tree, Vector3 location)
    {
        tree.transform.position = location;
        tree.transform.rotation = Quaternion.Euler(80, 0, 0);
        tree.AddComponent<BoxCollider>();
        tree.GetComponent<BoxCollider>().isTrigger = true;
        tree.transform.parent = parentObject.transform;
    }

    public void GenerateTreeLocs()
    {
        GenerateMapWithLocations(10, 30, "tree");
    }

    public void GenerateGreeneryLocs()
    {
        GenerateMapWithLocations(9, 70, "greenery");
    }

    private void GenerateMapWithLocations(int amountOfDifferentSpriteTypes, int amountOfLocsPerSpriteType, string spriteGroupName)
    {
        for (int j = 0; j < amountOfDifferentSpriteTypes; j++)
        {
            List<Vector3> objLocs = new List<Vector3>();
            for (int i = 0; i < amountOfLocsPerSpriteType; i++)
            {
                objLocs.Add(new Vector3(Random.Range(-1 * worldMaxX, worldMaxX), 1,
                    Random.Range(-1 * worldMaxY, worldMaxY)));
            }

            environmentObjectLocations.Add(spriteGroupName + "_" + j, objLocs);
        }
    }
}