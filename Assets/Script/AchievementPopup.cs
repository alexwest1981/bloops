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
        // Sätt popup-panelen som inaktiv vid start
        achievementPopupPanel.SetActive(false);
    }

    // Visa prestation popup med den upplåsta prestationstiteln och ikonen
    public void ShowAchievementPopup(Sprite achievementIcon, string achievementTitle)
    {
        // Visa popup-panelen
        achievementPopupPanel.SetActive(true);

        // Uppdatera ikonen för prestationen
        if (achievementIcon != null)
        {
            achievementIconImage.sprite = achievementIcon;
            achievementIconImage.gameObject.SetActive(true);
        }
        else
        {
            achievementIconImage.gameObject.SetActive(false);
        }

        // Uppdatera texten för prestationstitel
        achievementTitleText.text = achievementTitle;

        // Starta en korutin för att dölja popupen efter 4 sekunder
        StartCoroutine(HideAchievementPopupAfterDelay());
    }

    private IEnumerator HideAchievementPopupAfterDelay()
    {
        // Vänta i 4 sekunder
        yield return new WaitForSeconds(4f);

        // Dölj popup-panelen
        achievementPopupPanel.SetActive(false);
    }

    // Hantera händelsen när en prestation låses upp
    private void HandleAchievementUnlocked(string achievementTitle)
    {
        // Hämta upplåsta prestationen från AchievementManager
        AchievementDataSO unlockedAchievement = AchievementManager.instance.achievementsList.Find(a => a.title == achievementTitle);

        // Visa prestation popup med den upplåsta prestationstiteln och den upplåsta ikonen
        ShowAchievementPopup(unlockedAchievement.unlockedSprite, achievementTitle);
    }
}
