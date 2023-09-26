using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float MovementSpeed = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        // Player Movement Controls (WASD)
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * MovementSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * MovementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * MovementSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * MovementSpeed;
        }
    }
}
