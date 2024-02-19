using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public float timer = 0f;
    private bool isTimerRunning = false;

    public float GetTimer()
    {
        return timer;
    }

    // Starta timern
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    // Stoppa timern
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // Återställ timern
    public void ResetTimer()
    {
        timer = 0f;
    }

    // Uppdatera timern varje frame om den är igång
    private void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            // Uppdatera text eller annan visuell representation av tiden om så önskas
        }
    }
}

