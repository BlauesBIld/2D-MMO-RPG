using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSwipeRoute : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform[] ControlPoints; // Array of control points for gizmos curve
    private Vector2 GizmosPosition; // Current position of the projectile
    private GameObject Player;
    private Vector3 Target;
    private Camera Cam;

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Cam = Camera.main;
        Player = GameObject.Find("Player");
        transform.position = Player.transform.position;
        Target = Cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = Target - Player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // rotation of curve
        transform.rotation = Quaternion.AngleAxis(rotationZ + (90), Vector3.forward);

        for (float t = 0; t <= 1; t += 0.05f)
        {
            GizmosPosition = Mathf.Pow(1 - t, 3) * ControlPoints[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * ControlPoints[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * ControlPoints[2].position +
                Mathf.Pow(t, 3) * ControlPoints[3].position;

            Gizmos.DrawSphere(GizmosPosition, 1f);
        }

        /*Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
            new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));
        Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
            new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
        */
    }
}
