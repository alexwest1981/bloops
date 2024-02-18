using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class MergeManager : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    [Header(" Actions")]
    public static Action<BloopType, Vector2> onMergeProcessed;
    public static Action onTotalMergeProcessed;
    public static event Action<int> onLevelChanged;

    [Header(" Settings")]
    Bloop lastSender;
    bool isHapticFeedbackEnabled = true;

    public TextMeshProUGUI currentMergeCountText;
    public TextMeshProUGUI totalMergeCountText;
    public LevelUpPopup levelUpPopup;

    public Slider levelProgressBarSlider;
    public TextMeshProUGUI currentLevelText;

    private const string HapticFeedbackKey = "HapticFeedbackEnabled";
    private const string TotalMergeCountKey = "TotalMergeCount";
    private int currentMergeCount = 0;
    private int totalMergeCount = 0;
    private int currentXP = 0;
    private int currentLevel = 1;
    //private int maxLevel = 100;
    private PlayerData playerData;
    public static MergeManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Bloop.onCollisionWithBloop += CollisionBetweenBloopsCallback;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    private void OnDestroy()
    {
        Bloop.onCollisionWithBloop -= CollisionBetweenBloopsCallback;
    }

    void Start()
    {
        LoadHapticFeedbackSetting();
        LoadTotalMergeCount();
        playerData = new PlayerData();
        LoadPlayerData(); // Ladda spelardata här
        levelProgressBarSlider.maxValue = CalculateNextLevelXP();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        levelProgressBarSlider.value = currentXP;
        currentLevelText.text = currentLevel.ToString();
        currentMergeCountText.text = "Current Merges: " + GetCurrentMergeCount();
        totalMergeCountText.text = "Total Merges: " + GetTotalMergeCount();
        SaveTotalMergeCount();
    }

    private void CollisionBetweenBloopsCallback(Bloop sender, Bloop otherBloop)
    {
        if (lastSender != null)
            return;

        lastSender = sender;

        ProcessMerge(sender, otherBloop);
    }

    private void ProcessMerge(Bloop sender, Bloop otherBloop)
    {
        if (sender == null || otherBloop == null)
        {
            Debug.LogWarning("En eller båda Bloop-objekten är null.");
            return;
        }
        if (isHapticFeedbackEnabled)
        {
            PlayHapticFeedback();
        }
        BloopType mergeBloopType = sender.GetBloopType();
        mergeBloopType += 1;

        Vector2 bloopSpawnPos = (sender.transform.position + otherBloop.transform.position) / 2;

        sender.Merge();
        otherBloop.Merge();

        GainXP(2);
        currentMergeCount++;
        totalMergeCount++;

        SavePlayerData();

        StartCoroutine(ResetLastSenderCoroutine());

        onTotalMergeProcessed?.Invoke();
        onMergeProcessed?.Invoke(mergeBloopType, bloopSpawnPos);
        levelProgressBarSlider.maxValue = CalculateNextLevelXP();
        UpdateUI();
    }

    public int GetCurrentMergeCount()
    {
        return currentMergeCount;
    }

    public int GetTotalMergeCount()
    {
        return totalMergeCount;
    }

    public void EnableHapticFeedback()
    {
        isHapticFeedbackEnabled = true;
        SaveHapticFeedbackSetting();
    }

    private void GainXP(int amount)
    {
        currentXP += amount;

        // Calculate the remaining XP needed to reach the next level
        int remainingXP = CalculateNextLevelXP() - currentXP;

        // Check for level up
        if (remainingXP <= 0)
        {
            LevelUp();
        }

        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        int nextLevelXP = CalculateNextLevelXP();
        while (currentXP >= nextLevelXP)
        {
            LevelUp();
            nextLevelXP = CalculateNextLevelXP();
        }
    }

    private void LevelUp()
    {
        // Reset the progress within the current level
        currentXP = 0;

        // Increase the level
        currentLevel++;

        Debug.Log("Level up! New level: " + currentLevel);

        // Visa gratulationspopup
        if (levelUpPopup != null)
        {
            levelUpPopup.ShowLevelUpPopup(currentLevel);
        }

        // Meddela prenumeranter om nivåförändringen
        LevelChanged(currentLevel);

        UpdateUI();
    }


    // Metod för att ändra nivån och meddela prenumeranter
    private void LevelChanged(int newLevel)
    {
        onLevelChanged?.Invoke(newLevel);
    }

    void PlayHapticFeedback()
    {
        // Kontrollera om enheten stöder haptic feedback
        if (SystemInfo.supportsVibration)
        {
            // Vibrera i 30 millisekunder
            Handheld.Vibrate();
        }
        else
        {
            Debug.LogWarning("Enheten stöder inte haptic feedback.");
        }
    }

    IEnumerator ResetLastSenderCoroutine()
    {
        yield return new WaitForEndOfFrame();
        lastSender = null;
    }

    public void DisableHapticFeedback()
    {
        isHapticFeedbackEnabled = false;
        SaveHapticFeedbackSetting();
        Debug.Log("Haptic feedback disabled.");
    }

    public void ToggleHapticFeedback(bool isOn)
    {
        Debug.Log("ToggleHapticFeedback - isOn: " + isOn);

        if (isOn)
        {
            EnableHapticFeedback();
            Debug.Log("Haptic feedback toggled ON.");
        }
        else
        {
            DisableHapticFeedback();
            Debug.Log("Haptic feedback toggled OFF.");
        }
    }

    private void SaveHapticFeedbackSetting()
    {
        PlayerPrefs.SetInt(HapticFeedbackKey, isHapticFeedbackEnabled ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Haptic feedback setting saved: " + isHapticFeedbackEnabled);
    }

    private void LoadHapticFeedbackSetting()
    {
        isHapticFeedbackEnabled = PlayerPrefs.GetInt(HapticFeedbackKey, 1) == 1;
        Debug.Log("Haptic feedback setting loaded: " + isHapticFeedbackEnabled);

        toggle.isOn = isHapticFeedbackEnabled;
    }

    private void SaveTotalMergeCount()
    {
        PlayerPrefs.SetInt(TotalMergeCountKey, totalMergeCount);
        PlayerPrefs.Save();
    }

    private void LoadTotalMergeCount()
    {
        totalMergeCount = PlayerPrefs.GetInt(TotalMergeCountKey, 0);
    }

    private int CalculateNextLevelXP()
    {
        float increasePercentage = 0.30f;
        float nextLevelXP = 100 * Mathf.Pow(1 + increasePercentage, currentLevel);
        return Mathf.RoundToInt(nextLevelXP);
    }

    private void LoadPlayerData()
    {
        currentXP = PlayerPrefs.GetInt("PlayerXP", 0);
        currentLevel = PlayerPrefs.GetInt("PlayerLevel", 1);

        // Meddela prenumeranter om den aktuella nivån
        LevelChanged(currentLevel);
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetInt("PlayerXP", currentXP);
        PlayerPrefs.SetInt("PlayerLevel", currentLevel);
        PlayerPrefs.Save();
    }

    public int GetCurrentXP()
    {
        return currentXP;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetCurrentPlayerLevel()
    {
        // Retrieve the player's level from PlayerPrefs. If not found, default to level 1.
        return PlayerPrefs.GetInt("PlayerLevel", 1);
    }
}
