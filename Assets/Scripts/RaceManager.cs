using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField]
    CheckPoint[] checkPoints;
    public int checkPointsCount;

    public static RaceManager instance;

    private void Awake()
    {
        checkPointsCount = checkPoints.Length;
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
    }

    void Update()
    {

    }
}
