using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CornerPlayerInterfaceController : MonoBehaviour
{
    public GameInterfaceController gameInterfaceController;

    public Slider healthBar;
    public Slider manaBar;
    public Slider missingManaBar;
    public Slider experienceBar;

    public GameObject maxHealthText;
    public GameObject currentHealthText;
    public GameObject maxManaText;
    public GameObject currentManaText;

    public GameObject levelText;

    public HealthController healthController;
    public ManaController manaController;
    public ExperienceController experienceController;

    private bool isMissingManaCoroutineRunning = false;
    // Update is called once per frame
    void Update()
    {
        if (manaController != null && healthController != null && gameInterfaceController.inGameInterface.activeInHierarchy)
        {
            currentHealthText.GetComponent<TextMeshProUGUI>().text = ((int)healthController.currentHealth).ToString();
            currentManaText.GetComponent<TextMeshProUGUI>().text = ((int)manaController.currentMana).ToString();


            healthBar.value = healthController.currentHealth;
            manaBar.value = manaController.currentMana;
        }
    }

    public void InitializeBars(HealthController healthController, ManaController manaController, ExperienceController experienceController)
    {
        this.healthController = healthController;
        this.manaController = manaController;
        this.experienceController = experienceController;

        maxHealthText.GetComponent<TextMeshProUGUI>().text = "/" + (int)healthController.maxHealth;
        maxManaText.GetComponent<TextMeshProUGUI>().text = "/" + (int)manaController.maxMana;
        levelText.GetComponent<TextMeshProUGUI>().text = experienceController.level.ToString();

        healthBar.maxValue = healthController.maxHealth;
        manaBar.maxValue = manaController.maxMana;

        experienceBar.maxValue = experienceController.maxExperience;
        experienceBar.value = experienceController.currentExperience;

        missingManaBar.maxValue = manaController.maxMana;
        missingManaBar.value = 0;
    }

    public void UpdateExperienceValue()
    {
        if (experienceController != null) 
        {
            if (experienceBar.maxValue != experienceController.maxExperience)
            {
                experienceBar.maxValue = experienceController.maxExperience;
                UpdateLevelText();
            }
            if (experienceBar.value != experienceController.currentExperience)
            {
                experienceBar.value = experienceController.currentExperience;
            }
        }
    }

    public void UpdateLevelText()
    {
        levelText.GetComponent<TextMeshProUGUI>().text = experienceController.level.ToString();
    }

    public IEnumerator ShowMissingMana(float missingMana)
    {
        if (isMissingManaCoroutineRunning)
        {
            yield break;
        }

        isMissingManaCoroutineRunning = true;

        missingManaBar.value = missingMana;
        for (int j = 0; j < 2; j++)
        {
            for (float i = 0f; i <= 1; i += 0.1f)
            {
                if (missingMana < manaBar.value)
                {
                    missingManaBar.fillRect.GetComponent<Image>().color = Color.clear;
                    isMissingManaCoroutineRunning = false;
                    yield break;
                }

                missingManaBar.fillRect.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.white, i);
                yield return new WaitForSeconds(.03f);
            }

            for (float i = 1f; i >= 0; i -= 0.1f)
            {
                if (missingMana < manaBar.value)
                {
                    missingManaBar.fillRect.GetComponent<Image>().color = Color.clear;
                    isMissingManaCoroutineRunning = false;
                    yield break;
                }

                missingManaBar.fillRect.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.white, i);
                yield return new WaitForSeconds(.03f);
            }
        }

        missingManaBar.value = 0;

        isMissingManaCoroutineRunning = false;
    }
}
