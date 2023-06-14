using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool _isPause = false;

    public GameObject _pauseMenuUI;

    private GameInput _input;

    private void OnEnable()
    {
        _input = new GameInput();
        _input.Player.Pause.performed += OpenPauseMenu;
        _input.Player.Pause.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Pause.performed -= OpenPauseMenu;
        _input.Player.Pause.Disable();
    }

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

    private void OpenPauseMenu(InputAction.CallbackContext context)
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

    public void Pause()
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