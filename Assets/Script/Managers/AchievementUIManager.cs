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

    // Referenser till ikonens och titelns TextMeshProUGUI-komponenter från AchievementIconContainer
    public TextMeshProUGUI iconTitleText;
    public Image iconImagePrefab; // Uppdaterat namn för att undvika namnkonflikt

    public void UpdateUI(AchievementDataSO achievement)
    {
        if (achievement.unlocked)
        {
            lockedPanel.SetActive(false);
            unlockedPanel.SetActive(true);

            titleText.text = achievement.title;
            descriptionText.text = achievement.description;
            pointsText.text = "Points: " + achievement.achievementScore;
            iconTitleText.text = achievement.title; // Uppdatera titeln för ikonen
            iconImage.sprite = achievement.unlockedSprite;
        }
        else
        {
            lockedPanel.SetActive(true);
            unlockedPanel.SetActive(false);

            // Visa ett frågetecken eller annan bild för låst prestation
            iconImagePrefab.sprite = achievement.lockedSprite; // Använd den uppdaterade referensen
        }
    }

    public void OnButtonClick()
    {
        // Implementera logik för vad som ska hända när knappen klickas på
    }
}
