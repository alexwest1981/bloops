using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementUIManager : MonoBehaviour
{
    public GameObject lockedPanel;
    public GameObject unlockedPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI pointsText;
    public Image iconImage;

    // Referenser till ikonens och titelns TextMeshProUGUI-komponenter fr�n AchievementIconContainer
    public TextMeshProUGUI iconTitleText;
    public Image iconImagePrefab; // Uppdaterat namn f�r att undvika namnkonflikt

    public void UpdateUI(AchievementDataSO achievement)
    {
        if (achievement.unlocked)
        {
            lockedPanel.SetActive(false);
            unlockedPanel.SetActive(true);

            titleText.text = achievement.title;
            descriptionText.text = achievement.description;
            pointsText.text = "Points: " + achievement.achievementScore;
            iconTitleText.text = achievement.title; // Uppdatera titeln f�r ikonen
            iconImage.sprite = achievement.unlockedSprite;
        }
        else
        {
            lockedPanel.SetActive(true);
            unlockedPanel.SetActive(false);

            // Visa ett fr�getecken eller annan bild f�r l�st prestation
            iconImagePrefab.sprite = achievement.lockedSprite; // Anv�nd den uppdaterade referensen
        }
    }

    public void OnButtonClick()
    {
        // Implementera logik f�r vad som ska h�nda n�r knappen klickas p�
    }
}
