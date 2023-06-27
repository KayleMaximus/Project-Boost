using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.IO;

public class Rocket : MonoBehaviour
{
    Rigidbody _rigidbody;
    AudioSource _audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float _levelLoadDelay = 10f;
    [SerializeField] AudioClip _mainEngine;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] AudioClip _successSound;
    [SerializeField] ParticleSystem _mainEnginePart;
    [SerializeField] ParticleSystem _deathPart;
    [SerializeField] ParticleSystem _successPart;
    [SerializeField] bool _isCollision = true;
    private GameInput _gameInput;

    enum State { Alive, Dying, Transcending }
    State _state = State.Alive;

    private static readonly ISaveClient _client = new CloudSaveClient();

    void Start()
    {
        _isCollision = true;
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_state == State.Alive)
        {
            RespondToThrustInput(_gameInput.Player.Thurst.ReadValue<Vector2>());
            RespondToRotateInput(_gameInput.Player.Movement.ReadValue<Vector2>());
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void OnEnable()
    {
        _gameInput = new GameInput();
        _gameInput.Player.Enable();
    }

    private void OnDisable()
    {
        _gameInput.Player.Disable();
    }

    private void RespondToThrustInput(Vector2 Up)
    {
        if (Up.y == 1)
        {
            ApplyThrust();
        }
        else
        {
            _mainEnginePart.Stop();
            _audioSource.Stop();
        }
    }

    private void RespondToRotateInput(Vector2 direction)
    {
        _rigidbody.freezeRotation = true; //take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (direction.x == -1f)
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (direction.x == 1f)
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        _rigidbody.freezeRotation = false; //resume physics control of rotation
    }

    private void ApplyThrust()
    {
        _rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!_mainEnginePart.isPlaying)
        {
            _mainEnginePart.Play();
        }
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_mainEngine);
        }
    }

    #region Game Logic
    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCollision = !_isCollision;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_state != State.Alive || !_isCollision) return;  //Ignore Collision when dead
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        _state = State.Transcending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_successSound);
        _successPart.Play();
        int tempLevelReach = PlayerPrefs.GetInt("lvReach") + 1;
        PlayerPrefs.SetInt("lvReach", tempLevelReach);
        //SavePlayerProgress();
        CloudSave(tempLevelReach);
        Invoke("LoadNextLevel", _levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        _state = State.Dying;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_deathSound);
        _deathPart.Play();
        _mainEnginePart.Stop();
        Invoke("ReloadLevel", _levelLoadDelay);
    }

    private void LoadNextLevel() 
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    private async void CloudSave(int levelReach)
    {
        //await _client.Save("lvReach", levelReach);
        await _client.Save("PlayerLevelData", new PlayerData { playerId = PlayerData.defaultPlayerId, levelReach = levelReach });
    }

    // Update this method with the player progress you want to save
    private string GetPlayerProgressData()
    {
        // Example: Saving player level progress
        int playerLevel = PlayerPrefs.GetInt("lvReach", 1);
        return JsonUtility.ToJson(playerLevel);
    }

    // Call this method whenever the player makes progress
    public void SavePlayerProgress()
    {
        string saveData = GetPlayerProgressData();
        string filePath = Application.dataPath + "/saveData.json";

        // Write the JSON data to a file
        File.WriteAllText(filePath, saveData);

        // Push the file to GitHub using Git command-line tool with token authentication
        string remoteUrl = "https://github.com/KayleMaximus/testApi.git"; // Replace with your remote repository URL
        string branch = "main"; // Replace with the branch name you want to push to
        string personalAccessToken = "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"; // Replace with your personal access token

        string gitCommands = $"-C \"{Application.dataPath}\" init" +
                             $" && -C \"{Application.dataPath}\" add saveData.json" +
                             $" && -C \"{Application.dataPath}\" commit -m \"Update player progress\"" +
                             $" && -C \"{Application.dataPath}\" remote add origin {remoteUrl}" +
                             $" && -C \"{Application.dataPath}\" push origin {branch} " +
                             $"\"Authorization: token {personalAccessToken}\"";

        System.Diagnostics.Process.Start("git", gitCommands);
    }
}
