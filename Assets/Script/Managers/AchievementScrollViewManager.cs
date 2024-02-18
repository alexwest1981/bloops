using UnityEngine;
using UnityEngine.UI;

public class AchievementScrollViewManager : MonoBehaviour
{
    public GameObject achievementIconPrefab;
    public RectTransform achievementContent;
    public AchievementDataSO[] achievements;

    void Start()
    {
        foreach (AchievementDataSO achievement in achievements)
        {
            // Skapa en instans av prefabben
            GameObject achievementIcon = Instantiate(achievementIconPrefab, achievementContent);

            // Hämta referensen till AchievementIconManager-komponenten
            AchievementIconManager iconManager = achievementIcon.GetComponent<AchievementIconManager>();

            // Uppdatera ikonen och titeln med den aktuella prestationens data
            iconManager.UpdateIcon(achievement);
        }
    }
}
