using UnityEngine;

public class AutoAttackController : MonoBehaviour
{
    private bool isLocal = false;

    public float defaultSpeed;
    public float defaultRange;

    public float magicDamage = 0f;
    public float physicalDamage = 0f;

    public Vector3 startPosition;

    private void Start()
    {
        InitializeValues();
        GetComponent<Rigidbody>().velocity = transform.up * defaultSpeed;
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= defaultRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider hitInfo)
    {
        EnemyBaseController enemy = hitInfo.GetComponent<EnemyBaseController>();
        if (enemy != null)
        {
            if (isLocal)
            {
                ClientSend.DealDamageToEnemy(enemy, physicalDamage, magicDamage);
            }

            Destroy(gameObject);
        }
    }

    private void InitializeValues()
    {
        defaultSpeed = 35f;
        defaultRange = 15f;
        startPosition = transform.position;
    }

    public void SetLocal(bool isLocal)
    {
        this.isLocal = isLocal;
    }

    public void SetDamage(float physicalDamage, float magicDamage)
    {
        this.physicalDamage = physicalDamage;
        this.magicDamage = magicDamage;
    }

    public void SetParamaters(bool isLocal, float magicDamage)
    {
        SetLocal(isLocal);
        SetDamage(0, magicDamage);
    }
}