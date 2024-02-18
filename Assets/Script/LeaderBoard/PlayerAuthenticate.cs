using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.UI;
using TMPro;

public class PlayerAuthenticate : MonoBehaviour
{
    public string PlayerId { get; private set; }
    public string PlayerName { get; private set; }

    [SerializeField] private TextMeshProUGUI welcomeText;
    [SerializeField] private TextMeshProUGUI versionText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoginCoroutine());
        UpdateWelcomeText();
        VersionText();
    }

    IEnumerator LoginCoroutine()
    {
        bool done = false;

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if(response.success)
            {
                Debug.Log("Connected");
                PlayerId = response.player_id.ToString();
                UpdateWelcomeText();
                done = true;

                Debug.Log("PlayerID: " + PlayerId);
            }
            else
            {
                Debug.Log("Error connecting player...");
                done = true;
            }
        });

        yield return new WaitWhile(() => done == true);
    }

    public void UpdateWelcomeText()
    {
        if (welcomeText != null)
        {
            welcomeText.text = "Welcome " + PlayerId;
        }

        // Subscribe to the event in PlayerLeaderboard
        PlayerLeaderboard.instance.OnPlayerNameChanged += () =>
        {
            // HandlePlayerNameChanged logic here if needed
            // This is where you would update PlayerName if it changes
            Debug.Log("PlayerName changed");
        };
    }
    public void VersionText()
    {
        if (versionText != null)
        {
            versionText.text = "Version " + Application.version;
        }

    }


}