using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public float baseHealthRegenerationPerSecond;
    public int healthRegenMultiplier;
    public static int MAXHEALTHREGENMULTIPLIER;

    public Slider healthBar;

    public void InitializeHealth(float maxHealth = 150f, float currentHealth = -1f, float healthRegen = 1f)
    {
        if (currentHealth == -1f)
        {
            currentHealth = maxHealth;
        }
        SetMaxHealthAndHealth(maxHealth, currentHealth);
        SetBasehealthRegenerationPerSecond(healthRegen);
        SetHealthRegenMultiplier(1);
    }

    public void SetMaxHealthAndHealth(float maxHealth, float health)
    {
        SetMaxHealth(maxHealth);
        SetCurrentHealth(health);
    }

    public void SetMaxHealth(float maxHealth, bool setHealthToo = false)
    {
        this.maxHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        if (setHealthToo)
        {
            SetCurrentHealth(maxHealth);
        }
    }

    public void SetCurrentHealth(float health)
    {
        currentHealth = health;
        healthBar.value = health;
    }

    public void SetBasehealthRegenerationPerSecond(float healthRegen)
    {
        baseHealthRegenerationPerSecond = healthRegen;
    }

    public void SetHealthRegenMultiplier(int multiplier)
    {
        healthRegenMultiplier = multiplier;
    }

    public void Regenerate()
    {
        float newCurrentHealth = currentHealth + baseHealthRegenerationPerSecond * healthRegenMultiplier * Time.deltaTime;

        if (newCurrentHealth <= maxHealth)
        {
            SetCurrentHealth(newCurrentHealth);
        }
        else
        {
            SetCurrentHealth(maxHealth);
        }
    }

    public void DeactivateRegeneration()
    {
        healthRegenMultiplier = 0;
    }

    public void ActivateRegeneration()
    {
        healthRegenMultiplier = 1;
    }

    public bool IsRegenerationActive()
    {
        return healthRegenMultiplier == 0;
    }
}