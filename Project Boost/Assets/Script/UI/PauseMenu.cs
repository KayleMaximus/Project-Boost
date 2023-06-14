using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool _isPause = false;

    public GameObject _pauseMenuUI;

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("Focus: " + focus);
        if (!focus)
        {
            Pause();
        }
        else
        {
            if (_isPause)
            {
                Resume();
            }
        }
    }

    private void OnApplicationPause(bool isInterupt)
    {
        Debug.Log("Pause: " + isInterupt);
        if (isInterupt)
        {
            Pause();
        }
        else
        {
            if (_isPause)
            {
                Resume();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _isPause = true;
    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _isPause = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lobby");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}