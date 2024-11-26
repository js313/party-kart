using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject PausePanel;
    [SerializeField]
    string mainMenuScene;
    bool isPaused;

    void Start()
    {
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpauseGame();
        }
    }

    public void PauseUnpauseGame()
    {
        isPaused = !isPaused;
        PausePanel.SetActive(isPaused);
        AudioListener.pause = isPaused;

        if (isPaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void OnExitPressed()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
