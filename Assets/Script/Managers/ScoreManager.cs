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
    public static event Action<int> onScoreUpdated; // Ny h�ndelse f�r att meddela n�r po�ngen uppdateras

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
        onScoreUpdated?.Invoke(score); // Meddela att po�ngen har uppdaterats
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

    // Metod f�r att h�mta den aktuella po�ngen
    public int GetCurrentScore()
    {
        return score;
    }

    // Metod f�r att h�mta po�ngen
    public int GetScore()
    {
        return score;
    }

    public int GetBestScore()
    {
        return bestScore; // Eller returnera v�rdet p� det variabeln som inneh�ller det b�sta resultatet.
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
