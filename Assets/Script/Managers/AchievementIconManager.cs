using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementIconManager : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public void UpdateIcon(AchievementDataSO achievement)
    {
        // Uppdatera ikonen och titeln med den aktuella prestationens data
        if (achievement != null)
        {
            iconImage.sprite = achievement.unlocked ? achievement.unlockedSprite : achievement.lockedSprite;
            titleText.text = achievement.title;
            descriptionText.text = achievement.description; // Uppdatera beskrivningen
        }
    }
}
