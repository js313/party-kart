using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject selectTrackPanel, selectRacerPanel, raceSetupPanel;
    [SerializeField]
    Image selectedTrackImage, selectedRacerImage;

    public static MainMenu instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        raceSetupPanel.SetActive(false);
        selectTrackPanel.SetActive(false);
        selectRacerPanel.SetActive(false);
        selectedTrackImage.sprite = RaceInfoManager.instance.selectedTrackImage;
        selectedRacerImage.sprite = RaceInfoManager.instance.selectedRacerImage;
    }

    public void ChangeSelectedTrack(Image selectedTrack)
    {
        selectedTrackImage.sprite = selectedTrack.sprite;
        RaceInfoManager.instance.selectedTrackImage = selectedTrack.sprite;
        CloseTrackSelectPanel();
    }

    public void ChangeSelectedRacer(Image selectedRacer)
    {
        selectedRacerImage.sprite = selectedRacer.sprite;
        RaceInfoManager.instance.selectedRacerImage = selectedRacer.sprite;
        CloseRacerSelectPanel();
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene(RaceInfoManager.instance.trackToLoad);
    }

    public void OpenRaceSetupPanel()
    {
        raceSetupPanel.SetActive(true);
    }

    public void CloseRaceSetupPanel()
    {
        raceSetupPanel.SetActive(false);
    }

    public void OpenTrackSelectPanel()
    {
        selectTrackPanel.SetActive(true);
    }

    public void CloseTrackSelectPanel()
    {
        selectTrackPanel.SetActive(false);
    }

    public void OpenRacerSelectPanel()
    {
        selectRacerPanel.SetActive(true);
    }

    public void CloseRacerSelectPanel()
    {
        selectRacerPanel.SetActive(false);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
