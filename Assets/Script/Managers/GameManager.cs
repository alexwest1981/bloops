using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public static GameManager instance;
    private TimerManager timerManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Application.targetFrameRate = 60;
        timerManager = GetComponent<TimerManager>();
    }

    private bool isBoostActive = false;
    private float boostMultiplier = 10.0f;

    [Header(" Settings ")]
    private GameState gameState;

    [Header(" Actions")]
    public static Action<GameState> onGameStateChanged;


    // Start is called before the first frame update
    void Start()
    {
        SetMenu();
        StartLevelTimer();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeText();
    }

    private void SetMenu()
    {
        SetGameState(GameState.Menu);
    }

    private void SetGame()
    {
        SetGameState(GameState.Game);
        StartLevelTimer();
    }

    private void StartLevelTimer()
    {
        if (timerManager != null)
        {
            timerManager.ResetTimer(); // Återställ timern i TimerManager
            timerManager.StartTimer(); // Starta timern
        }
        else
        {
            Debug.LogError("TimerManager reference is null. Make sure TimerManager is attached to the GameManager object.");
        }
    }


    private void UpdateTimeText()
    {
        if (timeText != null && timerManager != null)
        {
            float currentTime = timerManager.timer; // Hämta den aktuella tiden från TimerManager
            timeText.text = FormatTime(currentTime);
        }
        else
        {
            Debug.LogError("TimeText reference is null or TimerManager is not assigned.");
        }
    }

    // Metod för att formatera tiden som visas i texten
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void SetGameover()
    {
        SetGameState(GameState.Gameover);
    }

    private void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState()
    {
        SetGame();
    }


    public bool IsGameState()
    {
        return gameState == GameState.Game;
    }

    public void WinGame()
    {
        Debug.Log("You win!");
        UIManager.ShowWinPanel(); // Visa vinstpanelen när spelet vinner
    }


    public void SetGameOverState()
    {
        SetGameover();
    }

    public bool IsBoostActive()
    {
        return isBoostActive;

    }

    public float GetBoostMultiplier()
    {
        return boostMultiplier;
    }

    public void ActivateBoost()
    {
        Debug.Log("Boost activated!");
        if (!isBoostActive)
        {
            StartCoroutine(BoostCoroutine());
        }
    }

    private IEnumerator BoostCoroutine()
    {
        isBoostActive = true;
        Debug.Log("BoostActive");
        boostMultiplier = 10.0f;
        yield return new WaitForSeconds(30);
        Debug.Log("Boost deactivated");
        isBoostActive = false;
    }
}
