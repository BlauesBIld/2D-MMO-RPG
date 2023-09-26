using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public int level;
    public int currentExperience;
    public static int baseMaxExperience = 100;
    public static int maxExperienceMultiplier = 10;
    public int maxExperience;

    public void Awake()
    {
        UpdateMaxExperience();
    }
    public void SetCurrentExperience(int experience)
    {
        currentExperience = experience;
        CheckAndUpdateLevel();
    }

    public void UpdateMaxExperience()
    {
        SetMaxExperience(baseMaxExperience + (level * maxExperienceMultiplier));
    }

    public void CheckAndUpdateLevel()
    {
        if (currentExperience >= maxExperience)
        {
            levelUp();
            currentExperience -= maxExperience;
            UpdateMaxExperience();
        }
    }

    public void levelUp()
    {
        level += 1;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void SetMaxExperience(int maxExperience)
    {
        this.maxExperience = maxExperience;
    }
}
