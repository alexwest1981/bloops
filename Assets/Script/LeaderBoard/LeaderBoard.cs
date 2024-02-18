using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using System;

public class LeaderBoard : MonoBehaviour
{
    public static LeaderBoard instance;

    //[Header(" Elements ")]
    //[SerializeField] private TextMeshProUGUI leaderboardText;

    [Header(" Settings ")]
    [SerializeField] private string leaderboardKey;

    public static Action<LootLockerLeaderboardMember[]> onLeaderboardFetched;

    private bool isLeaderboardLoaded = false;
    //private bool done = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Debug.Log("LeaderBoard Awake: Instance set to " + instance.gameObject.name);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("LeaderBoard Awake: Duplicate instance destroyed for " + gameObject.name);
        }

    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while(!LootLockerSDKManager.CheckInitialized())
        {
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("LootLocker SDK Initialized");
        FetchScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SubmitScore(string memberId, int score)
    {
        StartCoroutine(SubmitScoreCoroutine(memberId, score));
    }

    IEnumerator SubmitScoreCoroutine(string memberId, int score)
    {
        bool done = false;
        LootLockerSDKManager.SubmitScore(memberId, score, leaderboardKey, (response) =>
        {

            if(response.success)
            {
                Debug.Log("score sent: " + score);
                done = true;
            }
            else
            {
                Debug.Log("Failed to submit scoe...");
                done = true;
            }
        });

        yield return new WaitWhile(() => done == true);
        yield return new WaitForSeconds(1);
        FetchScores();
    }

    [NaughtyAttributes.Button]
    private void FetchScores()
    {
        Debug.Log("FetchScores called");
        // Kontrollera om leaderboard redan har laddats
        if (!isLeaderboardLoaded)
        {
            StartCoroutine(FetchScoreCoroutine());
        }
    }


    IEnumerator FetchScoreCoroutine()
    {
        bool done = false;
        int count = 50;

        LootLockerSDKManager.GetScoreList(leaderboardKey, count, 0, (response) =>
        {
            if(response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;

                onLeaderboardFetched?.Invoke(members);
                Debug.Log("onLeaderboardFetched invoked in LeaderBoard.cs");
                /*
                leaderboardText.text = "Names - Scores\n";

                for (int i = 0; i < members.Length; i++)
                {
                    string playerName = GetPlayerName(members[i]);
                    leaderboardText.text += playerName + " - " + members[i].score + "\n";
                }
                */

                done = true;
            }
            else
            {
                Debug.LogError("Failed to fetch leaderboard: ");
                done = true;
            }
        });

        yield return new WaitWhile(() => done == true);

        yield return new WaitForSeconds(1);
    }

    
}
