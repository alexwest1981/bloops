using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public class PlayerData
{
    public int currentXP;
    public int currentLevel;

    public void GainXP(int amount)
    {
        currentXP += amount;
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        int nextLevelXP = CalculateNextLevelXP();
        while (currentXP >= nextLevelXP)
        {
            LevelUp();
            nextLevelXP = CalculateNextLevelXP();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        Debug.Log("Level up! New level: " + currentLevel);
    }

    public int CalculateNextLevelXP()
    {
        float increasePercentage = 0.30f;
        float nextLevelXP = 100 * Mathf.Pow(1 + increasePercentage, currentLevel);
        return Mathf.RoundToInt(nextLevelXP);
    }

}
