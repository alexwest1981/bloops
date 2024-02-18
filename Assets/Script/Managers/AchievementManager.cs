using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    // Enkel delegat för att meddela när ett achievement låses upp
    public static event Action<string> onAchievementUnlocked;

    // Statisk instans för enkel åtkomst från andra skript
    public static AchievementManager instance;

    // Lista med prestationer
    public List<AchievementDataSO> achievementsList = new List<AchievementDataSO>();

    // Lista med namn på upplåsta prestationer
    private List<string> unlockedAchievements = new List<string>();

    // Nyckel för att spara upplåsta prestationer i PlayerPrefs
    private const string UnlockedAchievementsKey = "UnlockedAchievements";
    private int currentScore; // Variabel för att hålla den aktuella poängen
    public static Action onTotalMergeProcessed;
    public GameObject achievementPopupPrefab;

    // Total poäng från upplåsta prestationer
    private int totalAchievementScore = 0;

    // Egendom för att returnera total poäng från upplåsta prestationer
    public int TotalAchievementScore { get { return totalAchievementScore; } }
    public TextMeshProUGUI totalScoreText;


    // Se till att det bara finns en instans av AchievementManager
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Ladda upplåsta prestationer vid start
        LoadUnlockedAchievements();

        // Prenumerera på händelser från MergeManager
        MergeManager.onMergeProcessed += HandleMergeProcessed;
        MergeManager.onTotalMergeProcessed += HandleTotalMergeProcessed;
        // Prenumerera på händelsen för nivåförändringar från MergeManager
        MergeManager.onLevelChanged += HandleLevelChanged;
        ScoreManager.onScoreUpdated += HandleCurrentScore;

        // Beräkna total poäng från upplåsta prestationer vid start
        CalculateTotalAchievementScore();
    }

    // Avregistrera händelser när objektet förstörs
    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= HandleMergeProcessed;
        MergeManager.onTotalMergeProcessed += HandleTotalMergeProcessed;
        MergeManager.onLevelChanged -= HandleLevelChanged;
        ScoreManager.onScoreUpdated -= HandleCurrentScore;
    }

    private void Start()
    {
        // Uppdatera den totala poängen vid start
        UpdateTotalScoreUI();
    }

    private void HandleMergeProcessed(BloopType mergeBloopType, Vector2 bloopSpawnPos)
    {
        int currentMergeCount = MergeManager.instance.GetCurrentMergeCount();

        foreach (AchievementDataSO achievement in achievementsList)
        {
            if (!achievement.unlocked && achievement.achievementType == AchievementType.CurrentMerges && currentMergeCount >= achievement.requirementValue)
            {
                achievement.unlocked = true;
                SaveUnlockedAchievement(achievement.title);
                onAchievementUnlocked?.Invoke(achievement.title);

                // Skapa en instans av prefaben
                GameObject achievementPopupInstance = Instantiate(achievementPopupPrefab);
                Debug.Log("Instantiation completed");
            }
        }
    }


    private void HandleTotalMergeProcessed()
    {
        // Hämta totalt antal sammanslagningar från MergeManager
        int totalMergeCount = MergeManager.instance.GetTotalMergeCount();
        Debug.Log("Total merge count: " + totalMergeCount);

        foreach (AchievementDataSO achievement in achievementsList)
        {
            //Debug.Log("Checking achievement: " + achievement.title);
            //Debug.Log("Requirement: " + achievement.requirementValue);

            if (!achievement.unlocked &&
                achievement.achievementType == AchievementType.TotalMerges &&
                totalMergeCount >= achievement.requirementValue)
            {
                // Om kraven uppfylls, lås upp prestationen
                achievement.unlocked = true;
                SaveUnlockedAchievement(achievement.title);
                onAchievementUnlocked?.Invoke(achievement.title);
                Debug.Log("Achievement unlocked: " + achievement.title);
                // Skapa en instans av prefaben
                GameObject achievementPopupInstance = Instantiate(achievementPopupPrefab);
                Debug.Log("Instantiation completed");
            }
        }
    }



    private void HandleCurrentScore(int newScore)
    {
        foreach (AchievementDataSO achievement in achievementsList)
        {
            if (!achievement.unlocked && achievement.achievementType == AchievementType.Score && newScore >= achievement.requirementValue)
            {
                achievement.unlocked = true;
                SaveUnlockedAchievement(achievement.title);
                onAchievementUnlocked?.Invoke(achievement.title);
                // Skapa en instans av prefaben
                GameObject achievementPopupInstance = Instantiate(achievementPopupPrefab);
                Debug.Log("Instantiation completed");
            }
        }
    }

    // Skapa en metod för att hantera nivåförändringar
    private void HandleLevelChanged(int newLevel)
    {
        HandleCurrentLevel();
    }

    private void HandleCurrentLevel()
    {
        int currentLevel = MergeManager.instance.GetCurrentLevel();
        Debug.Log("Current level: " + currentLevel);

        foreach (AchievementDataSO achievement in achievementsList)
        {
            Debug.Log("Checking achievement: " + achievement.title);
            Debug.Log("Requirement: " + achievement.requirementValue);

            if (!achievement.unlocked &&
                achievement.achievementType == AchievementType.Level &&
                currentLevel >= achievement.requirementValue)
            {
                // Om kraven uppfylls, lås upp prestationen
                achievement.unlocked = true;
                SaveUnlockedAchievement(achievement.title);
                onAchievementUnlocked?.Invoke(achievement.title);
                Debug.Log("Achievement unlocked: " + achievement.title);
                // Skapa en instans av prefaben
                GameObject achievementPopupInstance = Instantiate(achievementPopupPrefab);
                Debug.Log("Instantiation completed");
            }
        }
    }


    // Metod för att spara en upplåst prestation i PlayerPrefs
    private void SaveUnlockedAchievement(string achievementName)
    {
        unlockedAchievements.Add(achievementName);
        SaveUnlockedAchievements();
    }

    // Metod för att beräkna total poäng från upplåsta prestationer
    private void CalculateTotalAchievementScore()
    {
        totalAchievementScore = 0;

        foreach (var achievement in achievementsList)
        {
            if (achievement.unlocked)
            {
                totalAchievementScore += achievement.achievementScore;
            }
        }
    }

    // Uppdatera framstegen för ett specifikt achievement och lås upp det om målet uppnås
    public static void UpdateAchievementProgress(string achievementName, int progress)
    {
        // Implementering av UpdateAchievementProgress...
    }

    // Kontrollera om ett specifikt achievement är upplåst
    public static bool IsAchievementUnlocked(string achievementName)
    {
        return instance.unlockedAchievements.Contains(achievementName);
    }

    // Spara upplåsta prestationer i PlayerPrefs
    private static void SaveUnlockedAchievements()
    {
        PlayerPrefs.SetString(UnlockedAchievementsKey, string.Join(",", instance.unlockedAchievements.ToArray()));
        PlayerPrefs.Save();
    }

    // Ladda upplåsta prestationer från PlayerPrefs
    private static void LoadUnlockedAchievements()
    {
        if (PlayerPrefs.HasKey(UnlockedAchievementsKey))
        {
            string[] unlockedAchievementsArray = PlayerPrefs.GetString(UnlockedAchievementsKey).Split(',');
            instance.unlockedAchievements.AddRange(unlockedAchievementsArray);
        }
    }
    private void UpdateTotalScoreUI()
    {
        // Hämta den totala poängen från AchievementManager och uppdatera texten
        int totalScore = AchievementManager.instance.TotalAchievementScore;
        totalScoreText.text = "Total Score: " + totalScore;
    }
}
