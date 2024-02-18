using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardMemberContainer : MonoBehaviour
{
    [Header(" Colors ")]
    private Color gold = new Color(1,1,0);
    private Color silver = new Color(0.6650944f,0.8954204f,0);
    private Color bronze = new Color(1, .5f, .25f);

    [Header(" Elements ")]
    [SerializeField] private Image rankContainer;
    [SerializeField] private TextMeshProUGUI ranktText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;


    public void Configure(int rank, string playerName, int score)
    {
        rankContainer.color = GetRankColor(rank);
        ranktText.text = rank.ToString();
        playerNameText.text = playerName;
        scoreText.text = score.ToString();
    }

    private Color GetRankColor(int rank)
    {
        switch (rank)
        {
            case 1: return gold;
            case 2: return silver;
            case 3: return bronze;
            default: return Color.gray;
        }
    }
}
