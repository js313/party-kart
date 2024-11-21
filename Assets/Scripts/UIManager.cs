using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text lapCounterDisplay, lapTimeDisplay, bestLapTimeDisplay;

    public static UIManager instance;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public void SetLapCounter(int lap)
    {
        lapCounterDisplay.text = lap + "/" + RaceManager.instance.totalLaps;
    }

    public void SetLapTime(float lapTime, bool isCurrentLap)
    {
        TimeSpan lapTimeSpan = TimeSpan.FromSeconds(lapTime);
        String formattedTimeString = String.Format("{0:00}m{1:00}.{2:000}s", lapTimeSpan.Minutes, lapTimeSpan.Seconds, lapTimeSpan.Milliseconds);
        if (isCurrentLap)
        {
            lapTimeDisplay.text = formattedTimeString;
        }
        else
        {
            bestLapTimeDisplay.text = formattedTimeString;
        }
    }
}
