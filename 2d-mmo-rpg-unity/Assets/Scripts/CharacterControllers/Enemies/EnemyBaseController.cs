using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBaseController : MonoBehaviour
{
    public int id;

    public string username;

    public float defaultSpeed = 5f;

    public Vector3 movingDirection; 
    
    private HealthController healthController;


    // Start is called before the first frame update
    // TODO virtual keyword to override the subclass and still call base function
    public virtual void Awake()
    {
        healthController = GetComponent<HealthController>();
    }
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = movingDirection * defaultSpeed;
    }

    public void Initialize(int id, float maxHealth, float currentHealth)
    {
        this.id = id;

        healthController.InitializeHealth(maxHealth, currentHealth);
    }

    public void Move( Vector3 position, Vector3 movingDirection)
    {
        transform.position = position;
        this.movingDirection = movingDirection;
    }

    public void SetHealth(float health)
    {
        healthController.SetCurrentHealth(health);
    }

        public virtual void Die()
        {
            Destroy(gameObject);
        }
}
