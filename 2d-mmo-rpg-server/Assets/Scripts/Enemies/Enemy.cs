using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Database;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    [NonSerialized] public int id;
    [NonSerialized] public float health;
    public float maxHealth = 100;

    public float physicalArmor;
    public float magicResist;

    public int experienceValue = 30;

    public Vector3 movingDirection = Vector3.zero;

    public List<Player> playersNearby = new List<Player>();

    public void Initialize(int id, float physicalArmor, float magicResist)
    {
        this.id = id;
        this.health = maxHealth;
        this.physicalArmor = physicalArmor;
        this.magicResist = magicResist;
    }

    public void TakeDamage(float magicDamage, float physicalDamage)
    {
        ProcessDamage(magicDamage, physicalDamage);
        if (health <= 0)
        {
            Die();
        }
    }

    private void ProcessDamage(float magicDamage, float physicalDamage)
    {
        health -= (magicDamage - magicResist + physicalDamage - physicalArmor);
        ServerSend.EnemyHealth(this);
    }

    public void Die()
    {
        EnemyManager.instance.enemies[id].GetComponentInParent<EnemySpawnerController>().currentAmountOfEnemiesAtATime--;
        EnemyManager.instance.enemies[id] = null;
        DistributeExperienceToNearbyPlayers();
        NotifyPlayers();
        Destroy(gameObject);
    }

    private void NotifyPlayers()
    {
        foreach (Player player in playersNearby)
        {
            ServerSend.RemoveEnemy(player.id, this);
        }
    }

    public void SendMovementAndPositionToPlayersNearby()
    {
        foreach (Player playerNearby in playersNearby)
        {
            ServerSend.EnemyServerMovement(playerNearby.id, this, movingDirection);
        }
    }

    public void SetPositionAndMovingDirection(Vector3 position, Vector3 movingDirection)
    {
        SetPosition(position);
        SetMovingDirection(movingDirection);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetMovingDirection(Vector3 movingDirection)
    {
        this.movingDirection = movingDirection;
    }

    public void DistributeExperienceToNearbyPlayers()
    {
        foreach (Player tplayer in playersNearby)
        {
            tplayer.AddExperience(experienceValue);
            DBExperience.UpdateLevelAndExpForUserChar(new ExperienceObj(tplayer.username, 1, tplayer.experienceController.level, tplayer.experienceController.currentExperience, tplayer.experienceController.maxExperience));
        }
    }

    public Player GetNearestPlayer()
    {
        float minDist = float.MaxValue;
        int nearestIndex = -1;
        for (int i = 0; i < playersNearby.Count; i++)
        {
            var dist = Vector3.Distance(transform.position, playersNearby[i].transform.position);
            if (dist < minDist)
            {
                nearestIndex = i;
                minDist = dist;
            }
        }

        return nearestIndex == -1 ? null : playersNearby[nearestIndex];
    }

    public Player GetRandomPlayer()
    {
        return playersNearby.Count <= 0 ? null : playersNearby[Random.Range(0, playersNearby.Count)];
    }

    public Quaternion GetAngleFromEnemyToNearestPlayer()
    {
        Player nearestPlayer = GetNearestPlayer();
        Quaternion directionToShoot = Quaternion.identity;
        
        if (nearestPlayer != null)
        {
            Vector3 nearestPlayerPosition = nearestPlayer.transform.position;
            float angleRad = Mathf.Atan2(nearestPlayerPosition.x - transform.position.x,
                nearestPlayerPosition.z - transform.position.z);
            float angleDeg = (180 / Mathf.PI) * angleRad;
            directionToShoot = Quaternion.Euler(90, angleDeg, 0);
        }
        
        return directionToShoot;
    }
}