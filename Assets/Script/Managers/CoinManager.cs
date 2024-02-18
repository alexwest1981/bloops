using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [Header(" Variables ")]
    private int coins;
    private const string coinsKey = "Coins";

    [Header(" Actions ")]
    public static Action onCoinsUpdated;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();

        UpdateCoinText();

        MergeManager.onMergeProcessed += MergeProcessedCallback;
    }

    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
    }

    private void MergeProcessedCallback(BloopType bloopType, Vector2 bloopSpawnPos)
    {
        int coinsToAdd = ((int)bloopType);
        AddCoins(coinsToAdd);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        coins = Mathf.Max(0, coins);

        SaveData();
        UpdateCoinText();
    }

    public int GetCoins()
    {
        return coins;
    }

    public bool CanPurchase(int price)
    {
        return coins >= price;
    }

    private void UpdateCoinText()
    {
        CoinText[] coinTexts = Resources.FindObjectsOfTypeAll(typeof(CoinText)) as CoinText[];

        for (int i = 0; i < coinTexts.Length; i++)
            coinTexts[i].UpdateText(coins.ToString());

        onCoinsUpdated?.Invoke();
    }

    private void LoadData() => coins = PlayerPrefs.GetInt(coinsKey);

    private void SaveData() => PlayerPrefs.SetInt(coinsKey, coins);
}
