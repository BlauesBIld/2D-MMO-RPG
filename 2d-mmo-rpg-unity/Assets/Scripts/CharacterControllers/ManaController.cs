using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaController : MonoBehaviour
{
    public float maxMana;
    public float currentMana;

    public float baseManaRegenerationPerSecond;
    public int manaRegenMultiplier;
    public static int MAXMANAREGENMULTIPLIER = 6;

    public Slider manaBar;

    public void InitializeMana(float maxMana = 100f, float manaRegen = 1f, float currentMana = -1f)
    {
        if (currentMana == -1f)
        {
            currentMana = maxMana;
        }
        setMaxManaAndCurrentMana(maxMana, currentMana);
        SetBaseManaRegenerationPerSecond(manaRegen);
        SetManaRegenMultiplier(1);
    }

    public void setMaxManaAndCurrentMana(float maxMana, float currentMana)
    {
        SetMaxMana(maxMana);
        SetCurrentMana(currentMana);
    }

    public void SetMaxMana(float maxMana, bool setCurrentManaToo = false)
    {
        this.maxMana = maxMana;
        manaBar.maxValue = maxMana;

        if (setCurrentManaToo)
        {
            SetCurrentMana(maxMana);
        }
    }

    public void SetCurrentMana(float currentMana)
    {
        this.currentMana = currentMana;
        manaBar.value = currentMana;
    }

    public void SetBaseManaRegenerationPerSecond(float manaRegen)
    {
        baseManaRegenerationPerSecond = manaRegen;
    }

    public void SetManaRegenMultiplier(int multiplier)
    {
        manaRegenMultiplier = multiplier;
    }

    public bool UseMana(float manaUsed)
    {
        if (EnoughManaFor(manaUsed))
        {
            SetCurrentMana(currentMana - manaUsed);
            ClientSend.PlayerMana(currentMana);
            return true;
        }

        return false;
    }

    private bool EnoughManaFor(float manaUsed)
    {
        return currentMana - manaUsed >= 0;
    }

    public void Regenerate()
    {
        float newCurrentMana = currentMana + baseManaRegenerationPerSecond * (manaRegenMultiplier*2) * Time.deltaTime;

        if (newCurrentMana <= maxMana)
        {
            SetCurrentMana(newCurrentMana);
        }
        else
        {
            SetCurrentMana(maxMana);
        }
    }

    public void DeactivateRegeneration()
    {
        manaRegenMultiplier = 0;
    }

    public void ActivateRegeneration()
    {
        manaRegenMultiplier = 1;
    }

    public bool IsRegenerationActive()
    {
        return manaRegenMultiplier != 0;
    }
}