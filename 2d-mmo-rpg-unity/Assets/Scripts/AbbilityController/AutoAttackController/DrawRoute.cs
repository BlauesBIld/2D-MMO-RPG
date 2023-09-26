using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DrawRoute : MonoBehaviour
{
    [SerializeField]
    public Transform[] ControlPoints; // Array of control points for gizmos curve
    Vector3 GizmosPosition; // Current position of the projectile

    public GameObject player;

    private void OnDrawGizmos()
    {
        transform.position = player.transform.position;
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = target - player.transform.position;
        float curveRotation = Mathf.Atan2(difference.z, difference.x) * Mathf.Rad2Deg; // rotation of curve
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, curveRotation, transform.eulerAngles.z);

        for (float t = 0; t <= 1; t += 0.05f)
        {
            GizmosPosition = Mathf.Pow(1 - t, 3) * ControlPoints[0].position +
                3 * Mathf.Pow(1 - t, 2)* t * ControlPoints[1].position +
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
