using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AchievementPopup : MonoBehaviour
{
    public TextMeshProUGUI achievementTitleText;
    public Image achievementIconImage;
    public GameObject achievementPopupPanel;

    private void OnEnable()
    {
        AchievementManager.onAchievementUnlocked += HandleAchievementUnlocked;
    }

    private void OnDisable()
    {
        AchievementManager.onAchievementUnlocked -= HandleAchievementUnlocked;
    }

    private void Start()
    {
        // S�tt popup-panelen som inaktiv vid start
        achievementPopupPanel.SetActive(false);
    }

    // Visa prestation popup med den uppl�sta prestationstiteln och ikonen
    public void ShowAchievementPopup(Sprite achievementIcon, string achievementTitle)
    {
        // Visa popup-panelen
        achievementPopupPanel.SetActive(true);

        // Uppdatera ikonen f�r prestationen
        if (achievementIcon != null)
        {
            achievementIconImage.sprite = achievementIcon;
            achievementIconImage.gameObject.SetActive(true);
        }
        else
        {
            achievementIconImage.gameObject.SetActive(false);
        }

        // Uppdatera texten f�r prestationstitel
        achievementTitleText.text = achievementTitle;

        // Starta en korutin f�r att d�lja popupen efter 4 sekunder
        StartCoroutine(HideAchievementPopupAfterDelay());
    }

    private IEnumerator HideAchievementPopupAfterDelay()
    {
        // V�nta i 4 sekunder
        yield return new WaitForSeconds(4f);

        // D�lj popup-panelen
        achievementPopupPanel.SetActive(false);
    }

    // Hantera h�ndelsen n�r en prestation l�ses upp
    private void HandleAchievementUnlocked(string achievementTitle)
    {
        // H�mta uppl�sta prestationen fr�n AchievementManager
        AchievementDataSO unlockedAchievement = AchievementManager.instance.achievementsList.Find(a => a.title == achievementTitle);

        // Visa prestation popup med den uppl�sta prestationstiteln och den uppl�sta ikonen
        ShowAchievementPopup(unlockedAchievement.unlockedSprite, achievementTitle);
    }
}
