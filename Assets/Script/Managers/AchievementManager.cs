using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    // Enkel delegat f�r att meddela n�r ett achievement l�ses upp
    public static event Action<string> onAchievementUnlocked;

    // Statisk instans f�r enkel �tkomst fr�n andra skript
    public static AchievementManager instance;

    // Lista med prestationer
    public List<AchievementDataSO> achievementsList = new List<AchievementDataSO>();

    // Lista med namn p� uppl�sta prestationer
    private List<string> unlockedAchievements = new List<string>();

    // Nyckel f�r att spara uppl�sta prestationer i PlayerPrefs
    private const string UnlockedAchievementsKey = "UnlockedAchievements";
    private int currentScore; // Variabel f�r att h�lla den aktuella po�ngen
    public static Action onTotalMergeProcessed;
    public GameObject achievementPopupPrefab;

    // Total po�ng fr�n uppl�sta prestationer
    private int totalAchievementScore = 0;

    // Egendom f�r att returnera total po�ng fr�n uppl�sta prestationer
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

        // Ladda uppl�sta prestationer vid start
        LoadUnlockedAchievements();

        // Prenumerera p� h�ndelser fr�n MergeManager
        MergeManager.onMergeProcessed += HandleMergeProcessed;
        MergeManager.onTotalMergeProcessed += HandleTotalMergeProcessed;
        // Prenumerera p� h�ndelsen f�r niv�f�r�ndringar fr�n MergeManager
        MergeManager.onLevelChanged += HandleLevelChanged;
        ScoreManager.onScoreUpdated += HandleCurrentScore;

        // Ber�kna total po�ng fr�n uppl�sta prestationer vid start
        CalculateTotalAchievementScore();
    }

    // Avregistrera h�ndelser n�r objektet f�rst�rs
    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= HandleMergeProcessed;
        MergeManager.onTotalMergeProcessed += HandleTotalMergeProcessed;
        MergeManager.onLevelChanged -= HandleLevelChanged;
        ScoreManager.onScoreUpdated -= HandleCurrentScore;
    }

    private void Start()
    {
        // Uppdatera den totala po�ngen vid start
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
        // H�mta totalt antal sammanslagningar fr�n MergeManager
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
                // Om kraven uppfylls, l�s upp prestationen
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

    // Skapa en metod f�r att hantera niv�f�r�ndringar
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
                // Om kraven uppfylls, l�s upp prestationen
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


    // Metod f�r att spara en uppl�st prestation i PlayerPrefs
    private void SaveUnlockedAchievement(string achievementName)
    {
        unlockedAchievements.Add(achievementName);
        SaveUnlockedAchievements();
    }

    // Metod f�r att ber�kna total po�ng fr�n uppl�sta prestationer
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

    // Uppdatera framstegen f�r ett specifikt achievement och l�s upp det om m�let uppn�s
    public static void UpdateAchievementProgress(string achievementName, int progress)
    {
        // Implementering av UpdateAchievementProgress...
    }

    // Kontrollera om ett specifikt achievement �r uppl�st
    public static bool IsAchievementUnlocked(string achievementName)
    {
        return instance.unlockedAchievements.Contains(achievementName);
    }

    // Spara uppl�sta prestationer i PlayerPrefs
    private static void SaveUnlockedAchievements()
    {
        PlayerPrefs.SetString(UnlockedAchievementsKey, string.Join(",", instance.unlockedAchievements.ToArray()));
        PlayerPrefs.Save();
    }

    // Ladda uppl�sta prestationer fr�n PlayerPrefs
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
        // H�mta den totala po�ngen fr�n AchievementManager och uppdatera texten
        int totalScore = AchievementManager.instance.TotalAchievementScore;
        totalScoreText.text = "Total Score: " + totalScore;
    }
}
