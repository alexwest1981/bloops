using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreAndCoinContainer : MonoBehaviour
{
    // Referens till MergeManager för att hämta information
    public MergeManager mergeManager;

    public Slider levelProgressBarSlider;
    public TextMeshProUGUI currentLevelText;

    void Start()
    {
        // Hämta information från MergeManager och uppdatera UI
        UpdateUI();
    }

    public void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        // Använd information från MergeManager för att uppdatera UI
        int currentXP = mergeManager.GetCurrentXP();
        int currentLevel = mergeManager.GetCurrentLevel();
        // Hämta andra relevanta uppgifter från MergeManager

        // Uppdatera UI med informationen
        levelProgressBarSlider.value = currentXP;
        currentLevelText.text = currentLevel.ToString();
    }
}
