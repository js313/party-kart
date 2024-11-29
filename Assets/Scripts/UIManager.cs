using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text lapCounterDisplay, lapTimeDisplay, bestLapTimeDisplay, positionDisplay, countDownDisplay, levelUnlocked;
    [SerializeField]
    float countDownDisappearAfter = 1f;

    [SerializeField]
    GameObject raceFinishedBG;
    [SerializeField]
    TMP_Text raceFinishedPosition;

    public static UIManager instance;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        raceFinishedBG.SetActive(false);
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

    public void SetPosition(int position)
    {
        positionDisplay.text = position + "/" + (RaceManager.instance.CompCarsCount + 1);
    }

    public void SetCountDown(int time)
    {
        if (time == 0)
        {
            countDownDisplay.text = "GO!";
            StartCoroutine(Disappear());

        }
        else
        {
            countDownDisplay.text = time.ToString();
        }
    }

    public void ShowRaceFinished(int playerPosition, bool trackUnlocked)
    {
        // Switch expression
        string positionSuffix = playerPosition switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th",
        };
        raceFinishedPosition.text = "You Finished " + playerPosition + positionSuffix + "!";
        levelUnlocked.enabled = trackUnlocked;
        raceFinishedBG.SetActive(true);
    }

    public void ExitRace(string sceneName)
    {
        RaceManager.instance.LoadScene(sceneName);
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(countDownDisappearAfter);
        countDownDisplay.enabled = false;
    }
}
