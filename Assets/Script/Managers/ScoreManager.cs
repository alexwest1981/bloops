using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private bool isBoostActive;
    private float boostMultiplier;
    public static event Action<int> onScoreUpdated; // Ny händelse för att meddela när poängen uppdateras

    [Header(" Elements")]
    [SerializeField] private TextMeshProUGUI gameScoreText;
    [SerializeField] private TextMeshProUGUI menuBestScoreText;
    [SerializeField] private TextMeshProUGUI menuBestScoreTextGameOver;

    [Header(" Settings")]
    [SerializeField] private float scoreMultiplier;
    private int score;
    private int bestScore;

    [Header(" Data")]
    private const string bestScoreKey = "bestScoreKey";


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
        MergeManager.onMergeProcessed += MergeProcessedCallback;

        GameManager.onGameStateChanged += GameStateChangedCallback;
    }
    
    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;

        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        UpdateBestScoreText();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Gameover:
                CalculateBestScore();
                break;
        }
    }

    private void MergeProcessedCallback(BloopType bloopType, Vector2 unused)
    {
        int scoreToAdd = (int)bloopType;
        score += Mathf.RoundToInt(scoreToAdd * (GameManager.instance.IsBoostActive() ? GameManager.instance.GetBoostMultiplier() : 5.0f));
        UpdateScoreText();
        onScoreUpdated?.Invoke(score); // Meddela att poängen har uppdaterats
    }



    private void UpdateScoreText()
    {
        gameScoreText.text = score.ToString();
    }

    private void UpdateBestScoreText()
    {
        menuBestScoreText.text = bestScore.ToString();
        menuBestScoreTextGameOver.text = bestScore.ToString();
    }

    private void CalculateBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            SaveData();
        }
    }

    // Metod för att hämta den aktuella poängen
    public int GetCurrentScore()
    {
        return score;
    }

    // Metod för att hämta poängen
    public int GetScore()
    {
        return score;
    }

    public int GetBestScore()
    {
        return bestScore; // Eller returnera värdet på det variabeln som innehåller det bästa resultatet.
    }

    public void SetBoostActive(bool isActive)
    {
        isBoostActive = isActive;
    }

    public void SetBoostMultiplier(float multiplier)
    {
        boostMultiplier = multiplier;
    }

    private void LoadData()
    {
        bestScore = PlayerPrefs.GetInt(bestScoreKey);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(bestScoreKey, bestScore);
    }   

}
