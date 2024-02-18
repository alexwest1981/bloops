using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerLeaderboard : MonoBehaviour
{
    public static PlayerLeaderboard instance;
    public event Action OnPlayerNameChanged;
    public PlayerAuthenticate PlayerAuthenticateInstance { get; private set; }



    [Header(" Elements ")]
    [SerializeField] private PlayerAuthenticate playerAuthenticate;
    [SerializeField] private TextMeshProUGUI welcomeText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        PlayerAuthenticateInstance = playerAuthenticate;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when the object is destroyed
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (!LootLockerSDKManager.CheckInitialized())
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("PlayerLeaderboard Start");
        UpdateWelcomeText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubmitScore()
    {
        string playerId = playerAuthenticate.PlayerId;
        int score = ScoreManager.instance.GetBestScore();
        LeaderBoard.instance.SubmitScore(playerId, score);
    }

    public void SetPlayerName(string playerName)
    {
        LootLockerSDKManager.SetPlayerName(playerName, (response) =>
        {
            if (response.success)
                Debug.Log("Player name has been set : " + playerName);
            else
                Debug.Log("Error setting the player name...");
        });
        SubmitScore();
        
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            
            case GameState.Gameover:
                SubmitScore();
                break;
        }
    }

    public void UpdateWelcomeText()
    {
        
        if (playerAuthenticate != null)
        {
            // Hämta spelarens namn från LootLocker för att säkerställa att det är uppdaterat
            LootLockerSDKManager.GetPlayerName((response) =>
            {
                if (response.success)
                {
                    Debug.Log("Successfully retrieved player name: " + response.name);
                    string playerName = response.name;

                    if (welcomeText != null)
                    {
                        welcomeText.text = "Welcome " + (playerName ?? "Unknown");
                    }

                    OnPlayerNameChanged?.Invoke(); // Anropa händelsen när namnet har hämtats
                }
                else
                {
                    Debug.LogError("Error fetching player info: " + response.errorData);
                }
            });
        }
        else
        {
            Debug.LogError("playerAuthenticate is null!");
        }
    }
}