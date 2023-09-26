using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffScript_1 : MonoBehaviour
{
    private Object BulletRef;
    private float AttackSpeed = 0.1f;
    public float TimeStart;
    private Transform Player;
    private List<Transform> Routes = new List<Transform>(); // List of routes (left and right side of player)
    private int RouteIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        RouteIndex = 0;
        GetRouteCurves();
        Player = transform.parent;
        BulletRef = Resources.Load("Bullet");
        TimeStart = AttackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        TimeStart -= Time.deltaTime;
        if (Input.GetMouseButton(0) & TimeStart <= 0)
        {
            GameObject bullet = (GameObject)Instantiate(BulletRef);
            bullet.transform.parent = gameObject.transform;
            bullet.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -1);
            TimeStart = AttackSpeed;
        }
    }

    public Transform GetRoute()
    {
        // Alternates between 2 routes
        RouteIndex = 1 - RouteIndex;
        return Routes[RouteIndex];
    }

    private void GetRouteCurves()
    {
        Routes.Add(transform.GetChild(0));
        Routes.Add(transform.GetChild(1));
    }
}
