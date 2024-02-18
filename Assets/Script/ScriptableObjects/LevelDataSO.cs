using UnityEngine;
using static LevelDataSO;

[CreateAssetMenu(fileName = "Level Data", menuName = "Scriptable Objects/Level Data", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public enum WinCriteria
    {
        Time,
        Score,
        BloopType // L�gg till detta alternativ f�r att v�lja blooptyp som vinstkriterium
    }


    [Header(" Data ")]
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private int requiredHighscore;
    [SerializeField] private int time; // Tid i sekunder innan banan �r slut ist�llet f�r coins required
    [SerializeField] private int coinsRequired;
    [SerializeField] private int requiredLevel; // Add this field
    [SerializeField] private WinCriteria winCriteria; // Vinstkriterium f�r niv�n

    public GameObject GetLevel() => levelPrefab;
    public int GetRequiredHighscore() => requiredHighscore;
    public int GetCoinsRequired() => coinsRequired;
    public int GetRequiredLevel() => requiredLevel; // Getter for required level
    public int GetTime() => time; // Getter for time

    public WinCriteria GetWinCriteria() => winCriteria;
}
