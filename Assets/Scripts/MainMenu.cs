using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public  void OnPlayPressed()
    {
        SceneManager.LoadScene(RaceInfoManager.instance.trackToLoad);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
