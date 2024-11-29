using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceInfoManager : MonoBehaviour
{
    public static RaceInfoManager instance;

    public string trackToLoad;
    public CarController racerToUse;
    public int noOfCompCars, noOfLaps;
    public float playerDefaultSpeed, playerTurnStrength;

    public Sprite selectedTrackImage, selectedRacerImage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UnlockTrack(trackToLoad);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
#endif
        }
    }

    public void UnlockTrack(string trackName)
    {
        PlayerPrefs.SetInt(trackName + "Unlocked", 1);
    }
}
