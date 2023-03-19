using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    void Start()
    {
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !InGameManager.instance.IsGameOver())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ToMainMenu()
    {
        SceneManager.instance.LoadMainMenu();
    }

    public void RestartGame()
    {
        InGameManager.instance.RestartGame();
        Resume();
    }

    public void OpenSettings()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OpenMain()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void CloseMain()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}

