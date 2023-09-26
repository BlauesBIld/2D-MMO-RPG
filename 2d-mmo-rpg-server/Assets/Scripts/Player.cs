using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public Transform shootOrigin;

    public float renderRadius = 10f;
    public float renderUpdateTime = 2f;
    public float renderTimer = 0f;
    
    public HealthController healthController;
    public ManaController manaController;
    public ExperienceController experienceController;
    
    public Inventory inventory = new Inventory();
    
    public List<Player> playersNearby = new List<Player>();
    public List<Enemy> enemiesNearby = new List<Enemy>();
    public List<GameObject> environmentObjectsNearby = new List<GameObject>();

    public void Awake()
    {
        healthController = GetComponent<HealthController>();
        manaController = GetComponent<ManaController>();
        experienceController = GetComponent<ExperienceController>();
    }

    public void Initialize(int id, string username, float maxHealth = 100f, float maxMana = 100f)
    {
        //TODO: Implement that health/maxHealth and mana/maxMana are sent from Client to Server
        this.id = id;
        this.username = username;
        this.healthController.SetMaxHealth(maxHealth, true);
        this.manaController.InitializeMana(maxMana, 1f);
    }

    public void Move(Vector3 movingDirection)
    {
        ServerSend.PlayerServerMovement(this, movingDirection);
    }

    public void SetPositionAndMovingDirection(Vector3 position, Vector3 movingDirection)
    {
        transform.position = position;
        Move(movingDirection);
    }

    public void Shoot(Quaternion direction, bool isRight)
    {
        ServerSend.PlayerShootAnim(this, direction, isRight);
    }

    public void SetMana(float currentMana)
    {
        manaController.SetCurrentMana(currentMana);
        ServerSend.PlayerServerMana(this);
    }

    public void SetHealth(float currentHealth)
    {
        healthController.SetCurrentHealth(currentHealth);
        ServerSend.PlayerServerHealth(this);
    }

    public void AddExperience(int experienceToAdd)
    {
        experienceController.SetCurrentExperience(experienceController.currentExperience + experienceToAdd);
        
        ServerSend.PlayerServerLevel(this);
        ServerSend.PlayerServerMaxExperience(this);
        ServerSend.PlayerServerCurrentExperience(this);
    }

    public void TellEnemiesNearbyImGone()
    {
        foreach (Enemy enemy in enemiesNearby)
        {
            enemy.playersNearby.Remove(this);
        }
    }
}
