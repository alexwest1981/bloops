using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;

public class LevelButton : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private TextMeshProUGUI levelIndexText;
    [SerializeField] private Button button;
    [SerializeField] private TMPro.TextMeshProUGUI requiredHighscoreText;


    private string buttonName;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = Random.ColorHSV(0f, 1f, .5f, 1f, .8f, 1f);
    }

    private int index;

    public void Configure(int levelIndex)
    {
        index = levelIndex;
        levelIndexText.text = levelIndex.ToString();
    }

    public int GetIndex()
    {
        return index;
    }

    public void SetInteractability(bool isInteractable)
    {
        button.interactable = isInteractable;
    }

    public void Enable() => button.interactable = true;

    public Button GetButton() => button;

    public bool IsCoinsButton()
    {
        // Add your logic here to determine if it's a coins button
        // For example, you can check if it's the coins button based on its properties or name.
        // Return true if it's a coins button, otherwise return false.
        // Replace the condition with your actual logic.

        // Example:
        return buttonName == "CoinsButton"; // Change "CoinsButton" to the actual name of your coins button.
    }


}
