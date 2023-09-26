using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    public StaffScript_1 StaffScript;
    private GameObject Player;
    public Transform Route;
    private bool RouteActive = true;
    private float GizmosTValue; // Distance of the projectile on the gizmos curve in percent
    private float RouteSpeed = 6f; //Speed of projectile during gizmos curve
    private Camera Cam;
    private Vector3 Target; // Mouse position
    private Rigidbody2D ProjectileRigidbody2D; // Rigidbody2D of projectile
    private float BulletSpeed = 500f; // Speed of the projectile when leaving the gizmos curve
    private GameObject Staff;

    // Start is called before the first frame update
    void Start()
    {
        Staff = GameObject.Find(transform.parent.name);
        StaffScript = (StaffScript_1)Staff.GetComponent(typeof(StaffScript_1));
        Cam = Camera.main;
        Target = Cam.ScreenToWorldPoint(Input.mousePosition);
        Player = GameObject.Find("Player");
        Route = StaffScript.GetRoute();
        ProjectileRigidbody2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (RouteActive)
        {
            StartCoroutine(GoByRoute(Route));
        }
        Destroy(gameObject, 0.75f);
    }

    private IEnumerator GoByRoute(Transform routeObject)
    {
        RouteActive = false;
        // Get position of gizmos curve control points
        Vector2 p0 = routeObject.GetChild(0).position;
        Vector2 p1 = routeObject.GetChild(1).position;
        Vector2 p2 = routeObject.GetChild(2).position;
        Vector2 p3 = routeObject.GetChild(3).position;

        while (GizmosTValue < 1)
        {
            GizmosTValue += Time.deltaTime * RouteSpeed;

            transform.position = Mathf.Pow(1 - GizmosTValue, 3) * p0 +
                3 * Mathf.Pow(1 - GizmosTValue, 2) * GizmosTValue * p1 +
                3 * (1 - GizmosTValue) * Mathf.Pow(GizmosTValue, 2) * p2 +
                Mathf.Pow(GizmosTValue, 3) * p3;

            yield return new WaitForEndOfFrame();
        }
        BulletToTarget();
        
    }

    private void BulletToTarget()
    {
        
        Vector3 difference = Target - Player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // for bullet angle
        Vector2 direction = difference / difference.magnitude;
        direction.Normalize();
        ProjectileRigidbody2D.velocity = direction * BulletSpeed;
    }
}
