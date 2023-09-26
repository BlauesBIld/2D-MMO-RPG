using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - transform.parent.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(GetComponentInParent<PlayerController>().movingDirection);
        transform.position = transform.parent.gameObject.transform.position + offset;
    }
}
