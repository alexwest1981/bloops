using UnityEngine;

public class WinGameManager : MonoBehaviour
{
    public bool CheckWinCriteria(LevelDataSO.WinCriteria winCriteria, int scoreThreshold, BloopType bloopTypeToMerge, Transform bloopsParent)
    {
        switch (winCriteria)
        {
            case LevelDataSO.WinCriteria.Time:
                // Implementera logik för tidsbaserade vinstkriterier här
                return false;
            case LevelDataSO.WinCriteria.Score:
                // Implementera logik för poängbaserade vinstkriterier här
                return ScoreManager.instance.GetCurrentScore() >= scoreThreshold;
            case LevelDataSO.WinCriteria.BloopType:
                // Implementera logik för bloop-typbaserade vinstkriterier här
                return CheckBloopTypeWinCriteria(bloopTypeToMerge, bloopsParent);
            default:
                return false;
        }
    }

    private bool CheckBloopTypeWinCriteria(BloopType bloopTypeToMerge, Transform bloopsParent)
    {
        foreach (Transform child in bloopsParent)
        {
            Bloop bloop = child.GetComponent<Bloop>();
            if (bloop != null && !bloop.HasCollided() && bloop.GetBloopType() == bloopTypeToMerge)
                return true;
        }
        return false;
    }
}
