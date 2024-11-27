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
}
