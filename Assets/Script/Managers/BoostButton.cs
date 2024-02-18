using UnityEngine;
using UnityEngine.UI;

public class BoostButton : MonoBehaviour
{
    public Button boostButton;
    public GameManager gameManager;

    void Start()
    {
        Button btn = boostButton.GetComponent<Button>();
        btn.onClick.AddListener(ShowRewardedVideo);
    }

    void ShowRewardedVideo()
    {
        if (AdsManager.Instance.IsRewardedVideoAvailable())
        {
            AdsManager.Instance.ShowRewardedVideo();
        }
        else
        {
            Debug.Log("Rewarded video not available");
        }
    }
}
