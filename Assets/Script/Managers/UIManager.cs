using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // Statisk instansvariabel för att hantera instansen av UIManager

    public GameObject winPanel; // Referens till vinstpanelen

    [Header(" Elements ")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject mapPanel;

    [Header(" Actions ")]
    public static Action onMapOpened;

    private void Awake()
    {
        // Tilldela instansen till 'instance' när UIManager skapas
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        GameManager.onGameStateChanged += GameStateChangedCallback;
        LevelMapManager.onLevelButtonClicked += LevelButtonCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        LevelMapManager.onLevelButtonClicked -= LevelButtonCallback;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Metod för att visa vinstpanelen
    public static void ShowWinPanel()
    {
        instance.winPanel.SetActive(true);
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                SetMenu();
                break;
            case GameState.Game:
                SetGame();
                break;
            case GameState.Gameover:
                SetGameover();
                break;
        }
    }

    private void SetMenu()
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameoverPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mapPanel.SetActive(false);
        shopPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }

    private void SetGame()
    {
        gamePanel.SetActive(true);
        menuPanel.SetActive(false);
        gameoverPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mapPanel.SetActive(false);
        shopPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }

    private void SetGameover()
    {
        gameoverPanel.SetActive(true);
        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        settingsPanel.SetActive(false);
        mapPanel.SetActive(false);
        shopPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }

    public void LevelButtonCallback()
    {
        mapPanel.SetActive(true);
        GameManager.instance.SetGameState();
        SetGame();
    }

    /*public void StartGame()
    {

        mapPanel.SetActive(false);
        GameManager.instance.SetGameState();
        SetGame();
    }*/

    public void RestartButtonCallback() => SceneManager.LoadScene(0);
    public void SettingsButtonCallback() => settingsPanel.SetActive(true);
    public void CloseSettingsPanel() => settingsPanel.SetActive(false);
    public void ShopButtonCallback() => shopPanel.SetActive(true);
    public void LeaderboardButtonCallback() => leaderboardPanel.SetActive(true);
    public void CloseShopPanel() => shopPanel.SetActive(false);
    public void CloseGamePanel() => gamePanel.SetActive(false);
    public void OpenMap()
    {
        mapPanel.SetActive(true);
        onMapOpened.Invoke();
    }
    public void CloseMap() => mapPanel.SetActive(false);
}
