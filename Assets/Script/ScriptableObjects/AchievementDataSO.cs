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
    public int requirementValue; // Kravv�rde f�r prestationen (t.ex. antal merges, po�ng, niv�)
    public int achievementScore; // Po�ng f�r att l�sa upp prestationen
    public bool unlocked; // Indikerar om prestationen �r l�st eller uppl�st
    //public int points; // Ytterligare po�ng f�r prestationen (om till�mpligt)
    public Sprite lockedSprite; // Sprite f�r l�st tillst�nd
    public Sprite unlockedSprite; // Sprite f�r uppl�st tillst�nd
}
