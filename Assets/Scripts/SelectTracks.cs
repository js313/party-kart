using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    TMP_Text lockedText;

    bool isUnlocked;

    private void Start()
    {
        SetLockStatus(PlayerPrefs.GetInt(sceneName + "Unlocked") == 0);
    }

    private void SetLockStatus(bool locked)
    {
        lockedText.enabled = locked;
        isUnlocked = !locked;
    }

    public void OnTrackSelect()
    {
        if (isUnlocked)
        {
            RaceInfoManager.instance.noOfLaps = totalLaps;
            RaceInfoManager.instance.trackToLoad = sceneName;
            MainMenu.instance.ChangeSelectedTrack(image);
        }
    }
}
