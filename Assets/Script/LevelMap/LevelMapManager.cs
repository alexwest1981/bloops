using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class LevelMapManager : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private RectTransform mapContent;
    [SerializeField] private RectTransform[] levelButtonParents;
    [SerializeField] private LevelButton levelButtonPrefab;
    [SerializeField] private GameObject unlockLevel;
    [SerializeField] private GameObject startLevel;

    [Header(" Data ")]
    [SerializeField] private LevelDataSO[] levelDatas;

    [Header(" Actions ")]
    public static Action onLevelButtonClicked;

    [Header(" UI ")]
    [SerializeField] private TextMeshProUGUI requiredScoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Button unlockButton;

    private LevelButton lastClickedLevelButton;

    private void Awake()
    {
        UIManager.onMapOpened += UpdateLevelButtonsInteractabillity;
        CoinManager.onCoinsUpdated += UpdateCoinText;
        unlockButton.onClick.AddListener(UnlockButtonClicked);
    }

    private void OnDestroy()
    {
        
        UIManager.onMapOpened -= UpdateLevelButtonsInteractabillity;
        CoinManager.onCoinsUpdated -= UpdateCoinText;
        unlockButton.onClick.RemoveListener(UnlockButtonClicked);
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        mapContent.anchoredPosition = Vector2.up * 1920 * (mapContent.childCount - 1);

        CreateLevelButtons();
        // UpdateLevelButtonsInteractabillity();
    }

    private void CreateLevelButtons()
    {
        for (int i = 0; i < levelDatas.Length; i++)
            CreateLevelButton(i, levelButtonParents[i]);
    }

    private void CreateLevelButton(int buttonIndex, Transform levelButtonParent)
    {
        LevelButton levelButton = Instantiate(levelButtonPrefab, levelButtonParent);
        levelButton.Configure(buttonIndex + 1);

        levelButton.GetButton().onClick.AddListener(() => LevelButtonClicked(buttonIndex)); // Line 76
    }

    private void LevelButtonClicked(int buttonIndex)
    {
        Debug.Log("Level button clicked: " + buttonIndex);

        while (transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        int bestScore = ScoreManager.instance.GetBestScore();
        int requiredHighscore = levelDatas[buttonIndex].GetRequiredHighscore();
        int coinsRequired = levelDatas[buttonIndex].GetCoinsRequired();
        int requiredLevel = levelDatas[buttonIndex].GetRequiredLevel();
        int playerLevel = MergeManager.instance.GetCurrentPlayerLevel(); // Use MergeManager to get the player's level

        if (requiredLevel > playerLevel)
        {
            Debug.Log("Showing unlockLevel panel");
            ShowPanel(unlockLevel); // Show unlockLevel panel
            coinText.text = levelDatas[buttonIndex].GetCoinsRequired().ToString();
        }
        else
        {
            Debug.Log("Showing startLevel panel");
            ShowPanel(startLevel); // Show startLevel panel
            Instantiate(levelDatas[buttonIndex].GetLevel(), transform);
            onLevelButtonClicked?.Invoke();
        }

        // Update TextMeshPro text to show the requirement
        requiredScoreText.text = requiredLevel.ToString();
    }


    private void ShowPanel(GameObject panel)
    {
        Debug.Log("Showing panel: " + panel.name);
        panel.SetActive(true);
        if (panel == unlockLevel)
        {
            startLevel.SetActive(false);
        }
        else if (panel == startLevel)
        {
            unlockLevel.SetActive(false);
        }
    }

    private void UpdateCoinText()
    {
        Debug.Log("Updating coin text...");

        int currentCoins = CoinManager.instance.GetCoins();
        Debug.Log("Current coins: " + currentCoins);

        coinText.text = currentCoins.ToString();

        if (lastClickedLevelButton != null)
        {
            int buttonIndex = lastClickedLevelButton.GetIndex();
            int coinsRequired = levelDatas[buttonIndex].GetCoinsRequired();

            // Update the required coins text in the UI
            requiredScoreText.text = "Required Coins: " + coinsRequired.ToString();
        }
        else
        {
            Debug.LogWarning("No level button clicked.");
            // Handle the situation where no level button was clicked.
        }
    }

    private void UnlockButtonClicked()
    {
        if (lastClickedLevelButton != null)
        {
            int buttonIndex = lastClickedLevelButton.GetIndex();
            int requiredHighscore = levelDatas[buttonIndex].GetRequiredHighscore();
            int coinsRequired = levelDatas[buttonIndex].GetCoinsRequired();

            if (CoinManager.instance.CanPurchase(coinsRequired))
            {
                CoinManager.instance.AddCoins(-coinsRequired);

                // Unlock the level and start it
                UnlockAndStartLevel(buttonIndex);
            }
            else
            {
                Debug.Log("Not enough coins to unlock the level.");
                // You can display a message to the player if they don't have enough coins.
            }
        }
        else
        {
            Debug.LogWarning("No level button clicked.");
            // Handle the situation where no level button was clicked.
        }
    }

    private void UnlockAndStartLevel(int buttonIndex)
    {
        // Additional actions when unlocking the level can be added here.

        // Show startLevel panel
        ShowPanel(startLevel);

        // Instantiate and start the selected level
        while (transform.childCount > 0)
        {
            Transform t = transform.GetChild(0);
            t.SetParent(null);
            Destroy(t.gameObject);
        }

        Instantiate(levelDatas[buttonIndex].GetLevel(), transform);
        onLevelButtonClicked?.Invoke();
    }



    private void UpdateLevelButtonsInteractabillity()
    {
        int bestScore = ScoreManager.instance.GetBestScore();

        for (int i = 0  ; i < levelDatas.Length; ++i)
            if (levelDatas[i].GetRequiredHighscore() <= bestScore)
                levelButtonParents[i].GetChild(0).GetComponent<LevelButton>().Enable();
    }
}
