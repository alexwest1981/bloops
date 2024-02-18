using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelProgressBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI levelText;

    public void SetMaxLevel(int maxLevel)
    {
        slider.maxValue = maxLevel;
        slider.value = 0; // �terst�ll v�rdet till 0 n�r du �ndrar maxv�rdet
        UpdateLevelText();
    }

    public void SetLevel(int level)
    {
        slider.value = level;
        UpdateLevelText();
    }

    public void SetXP(float xp, int maxXP)
    {
        slider.value = xp;
        slider.maxValue = maxXP;
        UpdateLevelText();
    }

    void UpdateLevelText()
    {
        levelText.text = "Level: " + slider.value.ToString();
    }
}
