using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Application.targetFrameRate = 60;
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetMenu()
    {
        SetGameState(GameState.Menu);
    }

    private void SetGame()
    {
        SetGameState(GameState.Game);
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
