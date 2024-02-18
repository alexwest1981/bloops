using UnityEngine;

public enum AchievementType
{
    CurrentMerges,
    TotalMerges,
    Score,
    Level
}

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
public class AchievementDataSO : ScriptableObject
{
    public string title;
    public string description;
    public AchievementType achievementType; // Typ av prestation
    public int requirementValue; // Kravvärde för prestationen (t.ex. antal merges, poäng, nivå)
    public int achievementScore; // Poäng för att låsa upp prestationen
    public bool unlocked; // Indikerar om prestationen är låst eller upplåst
    //public int points; // Ytterligare poäng för prestationen (om tillämpligt)
    public Sprite lockedSprite; // Sprite för låst tillstånd
    public Sprite unlockedSprite; // Sprite för upplåst tillstånd
}
