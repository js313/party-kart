using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTracks : MonoBehaviour
{
    [SerializeField]
    int totalLaps;
    [SerializeField]
    string sceneName;
    [SerializeField]
    Image image;

    public void OnTrackSelect()
    {
        RaceInfoManager.instance.noOfLaps = totalLaps;
        RaceInfoManager.instance.trackToLoad = sceneName;
        MainMenu.instance.ChangeSelectedTrack(image);
    }
}
