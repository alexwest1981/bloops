using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreAndCoinContainer : MonoBehaviour
{
    // Referens till MergeManager f�r att h�mta information
    public MergeManager mergeManager;

    public Slider levelProgressBarSlider;
    public TextMeshProUGUI currentLevelText;

    void Start()
    {
        // H�mta information fr�n MergeManager och uppdatera UI
        UpdateUI();
    }

    public void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        // Anv�nd information fr�n MergeManager f�r att uppdatera UI
        int currentXP = mergeManager.GetCurrentXP();
        int currentLevel = mergeManager.GetCurrentLevel();
        // H�mta andra relevanta uppgifter fr�n MergeManager

        // Uppdatera UI med informationen
        levelProgressBarSlider.value = currentXP;
        currentLevelText.text = currentLevel.ToString();
    }
}
