using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceController : MonoBehaviour
{
    public int level;
    public int maxExperience;
    
    public int currentExperience;


    public void SetCurrentExperience(int experience)
    {
        currentExperience = experience;
        UIManager.instance.GetCornerPlayerInterfaceController.UpdateExperienceValue();
    }

    public void SetMaxExperience(int maxExperience)
    {
        this.maxExperience = maxExperience;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void InitializeExp(int maxExp = 100, int currentExp = 0)
    {
        maxExperience = maxExp;
        currentExperience = currentExp;
    }
}
