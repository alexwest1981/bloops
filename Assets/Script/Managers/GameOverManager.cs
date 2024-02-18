using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject deadLine;
    [SerializeField] private Transform bloopsParent;

    [Header("Timer")]
    [SerializeField] private float durationThreshold;
    private float timer;
    private bool timerOn;
    private bool isGameOver;

    [Header("Win Criteria")]
    [SerializeField] private LevelDataSO.WinCriteria winCriteria;
    [SerializeField] private int scoreThreshold;
    [SerializeField] private BloopType bloopTypeToMerge;

    private void Update()
    {
        if (!isGameOver)
            ManageGameover();
    }

    private void ManageGameover()
    {
        if (timerOn)
            ManageTimerOn();
        else if (IsBloopAboveLine())
            StartTimer();
        else if (CheckWinCriteria())
        {
            Debug.Log("Time is up!"); // Lägg till denna rad för att övervaka när tiden går ut
            WinGame();
        }
    }

    private void ManageTimerOn()
    {
        timer += Time.deltaTime;
        Debug.Log("Timer: " + timer); // Lägg till denna rad för att övervaka timer-värdet

        if (!IsBloopAboveLine())
            StopTimer();

        if (timer >= durationThreshold)
        {
            Debug.Log("Time is up!"); // Lägg till denna rad för att kontrollera om tiden har gått ut korrekt
            GameOver();
        }
    }

    private bool IsBloopAboveLine()
    {
        foreach (Transform child in bloopsParent)
        {
            Bloop bloop = child.GetComponent<Bloop>();
            if (bloop != null && bloop.HasCollided() && IsBloopAboveLine(child))
                return true;
        }
        return false;
    }

    private bool IsBloopAboveLine(Transform bloop)
    {
        return bloop.position.y > deadLine.transform.position.y;
    }

    private void StartTimer()
    {
        timer = 0;
        timerOn = true;
    }

    private void StopTimer()
    {
        timerOn = false;
    }

    private void GameOver()
    {
        isGameOver = true;
        GameManager.instance.SetGameOverState();
    }

    private bool CheckWinCriteria()
    {
        switch (winCriteria)
        {
            case LevelDataSO.WinCriteria.Time:
                return false; // Time-based win criteria handled separately
            case LevelDataSO.WinCriteria.Score:
                return ScoreManager.instance.GetCurrentScore() >= scoreThreshold;
            case LevelDataSO.WinCriteria.BloopType:
                return CheckBloopTypeWinCriteria();
            default:
                return false;
        }
    }

    private bool CheckBloopTypeWinCriteria()
    {
        foreach (Transform child in bloopsParent)
        {
            Bloop bloop = child.GetComponent<Bloop>();
            if (bloop != null && !bloop.HasCollided() && bloop.GetBloopType() == bloopTypeToMerge)
                return true;
        }
        return false;
    }

    private void WinGame()
    {
        isGameOver = true;
        GameManager.instance.WinGame();
    }
}
