using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class Rocket : MonoBehaviour
{
    Rigidbody _rigidbody;
    AudioSource _audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float _levelLoadDelay = 2f;
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
            StartSuccessSequence();
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

}
