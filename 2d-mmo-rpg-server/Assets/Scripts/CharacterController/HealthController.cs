using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public float baseHealthRegenerationPerSecond;
    public int healthRegenMultiplier;
    public static int MAXHEALTHREGENMULTIPLIER;
    public void SetMaxHealthAndHealth(float maxHealth, float health)
    {
        SetMaxHealth(maxHealth);
        SetCurrentHealth(health);
    }

    public void SetMaxHealth(float maxHealth, bool setHealthToo = false)
    {
        this.maxHealth = maxHealth;
        if (setHealthToo)
        {
            SetCurrentHealth(maxHealth);
        }
    }

    public void SetCurrentHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
    }

    public bool AddMultiplierToBaseHealthRegen(int multiplier)
    {
        if (IsRegenerationActive())
        {
            if (healthRegenMultiplier + multiplier > MAXHEALTHREGENMULTIPLIER)
            {
                return false;
            }
            else
            {
                healthRegenMultiplier += multiplier;
                return true;
            }
        }

        return false;
    }

    public bool RemoveMultiplierFromBaseHealthRegen(int multiplier)
    {
        if (IsRegenerationActive())
        {
            if (healthRegenMultiplier - multiplier < 1)
            {
                healthRegenMultiplier = 1;
                return false;
            }
            else
            {
                healthRegenMultiplier -= multiplier;
                return true;
            }
        }

        return false;
    }

    public bool IsRegenerationActive()
    {
        return healthRegenMultiplier == 0;
    }
}
