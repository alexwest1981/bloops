using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    [Header(" Settings ")]
    [SerializeField] private string appKey;
    [SerializeField] TMP_Text _debugText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

    }

    private void OnDestroy()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent -= SdkInitializationCompletedEvent;
    }

    // Start is called before the first frame update
    void Start()
    {
        // IronSource.Agent.setMetaData("is_test_suite", "enable");

        IronSource.Agent.init(
            appKey,
            IronSourceAdUnits.REWARDED_VIDEO
            );

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SdkInitializationCompletedEvent()
    {
        // IronSource.Agent.launchTestSuite();
    }

    /*public void ShowRewardedVideo()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
            IronSource.Agent.showRewardedVideo();
        else
            Debug.Log("Rewarded video not available");
    }*/

    

    public void ShowRewardedVideo()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            _debugText.text = "Showing Ads...";
            IronSource.Agent.showRewardedVideo();
        }
        else
        {
            _debugText.text = "No Ads Available...";
            Debug.Log("Rewarded video not available");
        }
    }


    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    /*void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }*/
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        // Check if the placement is the one you expect (if you have multiple placements)
        if (placement != null && placement.getPlacementName().Equals("DefaultRewardedVideo"))
        {
            // Call the same logic as TNTButtonCallback
            ApplyDetonatorPowerUp();
        }
    }

    void ApplyDetonatorPowerUp()
    {
        // Your existing logic from TNTButtonCallback
        Bloop[] largeBloops = BloopManager.Instance.GetLargeBloops();

        if (largeBloops.Length <= 0)
            return;

        for (int i = 0; i < largeBloops.Length; i++)
            largeBloops[i].Merge();

        // CoinManager.instance.AddCoins(-tntPrice);
    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }

    public bool IsRewardedVideoAvailable()
    {
        return IronSource.Agent.isRewardedVideoAvailable();
    }

}
