using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int id;

    public string username;

    public GameObject autoAttackPrefab;
    public Vector3 movingDirection = Vector3.zero;

    private HealthController healthController;

    private ManaController manaController;

    private ExperienceController experienceController;

    private float speed = 20f;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = movingDirection.normalized * speed;
    }

    public void Initialize(int id, string username, float maxhealth = 150f, float baseMana = 100f, float speed = 20f)
    {
        this.id = id;
        this.username = username;
        GetComponentInChildren<TextMeshPro>().text = username;

        InitializeHealthAndManaController(maxhealth, baseMana);

        this.speed = speed;
    }

    public void InitializeExp(int maxExp = 100, int currentExp = 0)
    {
        if ((experienceController = GetComponent<ExperienceController>()) != null)
        {
            experienceController.InitializeExp(maxExp, currentExp);
        }
    }

    private void InitializeHealthAndManaController(float maxhealth, float baseMana)
    {
        InitializeHealthController(maxhealth);

        InitializeManaController(baseMana);
    }

    private void InitializeHealthController(float maxhealth)
    {
        healthController = GetComponent<HealthController>();
        if (healthController == null)
        {
            Debug.Log("Error while trying to load HealthController, did you add the HealthController Component?");
            Destroy(this);
        }

        healthController.InitializeHealth(maxhealth);
    }

    private void InitializeManaController(float baseMana)
    {
        manaController = GetComponent<ManaController>();
        if (manaController == null)
        {
            Debug.LogError("[MageController] Missing required ManaController! Aborting Creation of GameObject.");
            Destroy(this);
        }

        manaController.InitializeMana(baseMana);
    }

    public void Move(Vector3 position, Vector3 movingDirection)
    {
        transform.position = position;
        this.movingDirection = movingDirection;
    }

    public void ChangeAbsolutePosition(Vector3 _position)
    {
        transform.position = _position;
    }
}