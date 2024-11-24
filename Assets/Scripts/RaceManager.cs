using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField]
    CheckPoint[] checkPoints;
    [HideInInspector]
    public int checkPointsCount;

    public int totalLaps;

    public static RaceManager instance;

    [SerializeField]
    List<CarController> allCompCars;
    List<float> compCarsMaxSpeed;
    public int CompCarsCount { get; private set; }

    [SerializeField]
    CarController playerCar;

    [SerializeField]
    float playerDefaultSpeed, rubberBandSpeedMod = 2f, rubberBandAcceleration = 0.01f;

    readonly float positionCheckTime = 0.3f;
    float positionCheckTimeNow = 0.0f;

    [SerializeField]
    int playerPosition = 1;

    public bool isStarting = true;
    float timeBetweenCounts = 1f, timeLeft = 0f;
    int startCounter = 3;

    private void Awake()
    {
        checkPointsCount = checkPoints.Length;
        CompCarsCount = allCompCars.Count;
        playerDefaultSpeed = playerCar.maxSpeed;

        if (!instance)
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].CheckPointIndex = i;
        }
        compCarsMaxSpeed = new List<float>();
        foreach (CarController compCar in allCompCars)
        {
            compCarsMaxSpeed.Add(compCar.maxSpeed);
        }
    }

    private void Update()
    {
        if (isStarting) timeLeft -= Time.deltaTime;
        if (isStarting && timeLeft <= 0f)
        {
            timeLeft = timeBetweenCounts;
            UIManager.instance.SetCountDown(startCounter);
            startCounter--;
            if (startCounter == -1) // Including "GO"
            {
                isStarting = false;
            }
        }
        if (isStarting) return;
        positionCheckTimeNow -= Time.deltaTime;

        if (positionCheckTimeNow <= 0f)
        {
            playerPosition = 1;
            foreach (CarController compCar in allCompCars)
            {
                if (compCar.GetLap() > playerCar.GetLap())
                {
                    playerPosition++;
                }
                else if (compCar.GetLap() == playerCar.GetLap() && compCar.GetNextCheckPoint() > playerCar.GetNextCheckPoint())
                {
                    playerPosition++;
                }
                else if (compCar.GetLap() == playerCar.GetLap() && compCar.GetNextCheckPoint() == playerCar.GetNextCheckPoint())
                {
                    float compCarDistToNextCheckPoint = Vector3.SqrMagnitude(compCar.transform.position - checkPoints[compCar.GetNextCheckPoint()].transform.position);
                    float playerCarDistToNextCheckPoint = Vector3.SqrMagnitude(playerCar.transform.position - checkPoints[playerCar.GetNextCheckPoint()].transform.position);
                    if (compCarDistToNextCheckPoint < playerCarDistToNextCheckPoint)
                    {
                        playerPosition++;
                    }
                }
            }

            UIManager.instance.SetPosition(playerPosition);
            positionCheckTimeNow = positionCheckTime;
        }

        // Rubber Banding
        // Could also add, increase the speed proportional to the amount of time spent in that position
        if (playerPosition == 1)
        {
            playerCar.maxSpeed = Mathf.MoveTowards(playerCar.maxSpeed, playerDefaultSpeed - rubberBandSpeedMod, rubberBandAcceleration);
            int index = 0;
            foreach (CarController compCar in allCompCars)
            {
                float compDefaultSpeed = compCarsMaxSpeed[index++];
                compCar.maxSpeed = Mathf.MoveTowards(compCar.maxSpeed, compDefaultSpeed + rubberBandSpeedMod, rubberBandAcceleration);
            }
        }
        else
        {
            playerCar.maxSpeed = Mathf.MoveTowards(playerCar.maxSpeed, playerDefaultSpeed + (rubberBandSpeedMod * playerPosition / ((float)CompCarsCount + 1)), rubberBandAcceleration);
            int index = 0;
            foreach (CarController compCar in allCompCars)
            {
                float compDefaultSpeed = compCarsMaxSpeed[index++];
                compCar.maxSpeed = Mathf.MoveTowards(compCar.maxSpeed, compDefaultSpeed - (rubberBandSpeedMod * playerPosition / ((float)CompCarsCount + 1)), rubberBandAcceleration);
            }
        }
    }

    public Vector3 GetCheckPointPosition(int index)
    {
        return checkPoints[index].transform.position;
    }
}
