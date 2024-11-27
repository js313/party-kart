using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRacers : MonoBehaviour
{
    [SerializeField]
    CarController carController;
    [SerializeField]
    Image image;

    public void OnRacerSelect()
    {
        RaceInfoManager.instance.racerToUse = carController;
        MainMenu.instance.ChangeSelectedRacer(image);
    }
}
