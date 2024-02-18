using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Button blastButton;
    [SerializeField] private Button tntButton;

    [Header(" Settings ")]
    [SerializeField] private int blastPrice;
    [SerializeField] private int tntPrice;

    private void Awake()
    {
        CoinManager.onCoinsUpdated += CoinsUpdatedCallback;
    }

    private void OnDestroy()
    {
        CoinManager.onCoinsUpdated -= CoinsUpdatedCallback;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlastButtonCallback()
    {
        Bloop[] smallBloops = BloopManager.Instance.GetSmallBloops();

        if (smallBloops.Length <= 0)
            return;

        for (int i = 0; i < smallBloops.Length; i++)
            smallBloops[i].Merge();

        CoinManager.instance.AddCoins(-blastPrice);
    }

    private void ManageBlastButtonInteractability()
    {
        bool canBlast = CoinManager.instance.CanPurchase(blastPrice);

        blastButton.interactable = canBlast;
    }

    private void CoinsUpdatedCallback()
    {
        ManageBlastButtonInteractability();
        ManageTNTButtonInteractactable();
    }

    public void TNTButtonCallback()
    {
        Bloop[] mediumBloops = BloopManager.Instance.GetMediumBloops();

        if (mediumBloops.Length <= 0)
            return;

        for(int i = 0;i < mediumBloops.Length;i++)
            mediumBloops[i].Merge();

        CoinManager.instance.AddCoins(-tntPrice);
    }

    private void ManageTNTButtonInteractactable()
    {
        bool canTNT = CoinManager.instance.CanPurchase(tntPrice);
        tntButton.interactable = canTNT;
    }

}
