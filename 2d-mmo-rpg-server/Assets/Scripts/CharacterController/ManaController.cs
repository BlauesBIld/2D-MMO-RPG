using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour
{
    public float maxMana;
    public float currentMana;

    public float baseManaRegenerationPerSecond;
    public int manaRegenMultiplier;
    public static int MAXMANAREGENMULTIPLIER = 6;

    public void InitializeMana(float maxMana = 100f, float manaRegen = 1f, float currentMana = -1f)
    {
        if (currentMana == -1f)
        {
            currentMana = maxMana;
        }

        SetMaxManaAndCurrentMana(maxMana, currentMana);
        SetBaseManaRegenerationPerSecond(manaRegen);
        SetManaRegenMultiplier(1);
    }

    private void SetBaseManaRegenerationPerSecond(float manaRegen)
    {
        baseManaRegenerationPerSecond = manaRegen;
    }

    public void SetMaxManaAndCurrentMana(float maxMana, float currentMana)
    {
        SetMaxMana(maxMana);
        SetCurrentMana(currentMana);
    }

    public void SetMaxMana(float maxMana, bool setCurrentManaToo = false)
    {
        this.maxMana = maxMana;
        if (setCurrentManaToo)
        {
            SetCurrentMana(maxMana);
        }
    }

    public void SetCurrentMana(float currentMana)
    {
        this.currentMana = currentMana;
    }

    public void SetManaRegenMultiplier(int manaRegenMultiplier)
    {
        this.manaRegenMultiplier = manaRegenMultiplier;
    }

    public bool AddToManaRegenMultiplier(int multiplier)
    {
        if (manaRegenMultiplier + multiplier > MAXMANAREGENMULTIPLIER)
        {
            return false;
        }
        else
        {
            manaRegenMultiplier += multiplier;
            return true;
        }
    }

    public bool RemoveMultiplierFromBaseManaRegen(int multiplier)
    {
        if (IsRegenerationActive())
        {
            if (manaRegenMultiplier - multiplier < 1)
            {
                return false;
            }
            else
            {
                manaRegenMultiplier -= multiplier;
                return true;
            }
        }

        return false;
    }

    public bool IsRegenerationActive()
    {
        return manaRegenMultiplier != 0;
    }

    public void TemporaryAddOneMultiplierToBaseManaRegen(float timeInSeconds)
    {
        if (AddToManaRegenMultiplier(1))
        {
            Debug.Log("Added 1!");
            ServerSend.PlayerSetMPRegenMultiplier(Server.clients[GetComponent<Player>().id].player);
            StartCoroutine(RemoveManaMultiplierAfterTime(timeInSeconds));
        }
    }

    IEnumerator RemoveManaMultiplierAfterTime(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        if (RemoveMultiplierFromBaseManaRegen(1))
        {
            Debug.Log("Removed 1!");
            ServerSend.PlayerSetMPRegenMultiplier(Server.clients[GetComponent<Player>().id].player);
        }
    }
}