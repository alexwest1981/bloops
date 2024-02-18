using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class LeaderboardUI : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private LeaderboardMemberContainer memberContainerPrefab;
    [SerializeField] private Transform memberContainersParent;

    private void Awake()
    {
        LeaderBoard.onLeaderboardFetched += LeaderboardFetchedCallback;
    }

    private void OnDestroy()
    {
        LeaderBoard.onLeaderboardFetched -= LeaderboardFetchedCallback;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LeaderboardFetchedCallback(LootLockerLeaderboardMember[] members)
    {
        Debug.Log("LeaderboardFetchedCallback in LeaderboardUI.cs called with members: " + members.Length);
        for (int i = 0; i < members.Length; i++)
        {
            if (memberContainersParent.childCount <= i)
                CreateMemberContainer(members[i]);
            else
            {
                LeaderboardMemberContainer container = memberContainersParent.GetChild(i).GetComponent<LeaderboardMemberContainer>();
                ConfigureContainer(container, members[i]);
            }
        }


        while(memberContainersParent.childCount > members.Length)
        {
            Transform t = memberContainersParent.GetChild(memberContainersParent.childCount-1);
            t.SetParent(null);
            Destroy(t.gameObject);
        }
    }

    private void ConfigureContainer(LeaderboardMemberContainer container, LootLockerLeaderboardMember member)
    {
        container.Configure(member.rank, GetPlayerName(member), member.score);
    }

    private void CreateMemberContainer(LootLockerLeaderboardMember member)
    {
        LeaderboardMemberContainer containerInstance = Instantiate(memberContainerPrefab, memberContainersParent);
        ConfigureContainer(containerInstance, member);
        Debug.Log("Container instantiated for member: " + member);
    }

    
    private string GetPlayerName(LootLockerLeaderboardMember member)
    {
        string playerName = "Player_" + member.member_id;

        if (member.player.name.Length > 0)
            playerName = member.player.name;
        return playerName;
    }
}
