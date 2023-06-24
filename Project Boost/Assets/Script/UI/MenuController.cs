using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
public class MenuController : MonoBehaviour
{
    #region Declaration
    [SerializeField] private SceneLoader _sceneLoader;

    [Header("Volume Setting")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TMP_Text _volumeTextValue = null;
    [SerializeField] private Slider _volumeSlider = null;
    [SerializeField] private float _defaultVolume = -40f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text _controllerSenTextValue = null;
    [SerializeField] private Slider _controllerSenSlider = null;
    [SerializeField] private int _defaultSen = 4;
    public int _mainControllerSen;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle _invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Text _brightnessTextValue = null;
    [SerializeField] private Slider _brightnessSlider = null;
    [SerializeField] private float _defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    [SerializeField] private int _defaultQualityIndex = 1;    //Medium
    [SerializeField] private Toggle _fullscreenToggle;
    

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown _resolutionDropdown;
    private Resolution[] _resolutions;

    [Header("Conformation")]
    [SerializeField] private GameObject _confirmationPrompt = null;


    #endregion

    private void Start()
    {
        GetResolutions();

        _sceneLoader = GameObject.Find("LevelLoader").GetComponent<SceneLoader>();

        if(_sceneLoader == null)
        {
            Debug.LogError("SceneLoader on MenuController is null!");
        }
    }

    #region Graphics Setting

    private void GetResolutions()
    {
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        _brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        //change brightness with post processing or whatever control light

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);
        
        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1:0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    #endregion

    #region Gameplay Setting

    public void SetControllerSen(float sensitivity)
    {
        _mainControllerSen = Mathf.RoundToInt(sensitivity);
        _controllerSenTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        if (_invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
        }
        else 
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
        }

        PlayerPrefs.SetFloat("masterSen", _mainControllerSen);
        StartCoroutine(ConfirmationBox());
    }
    #endregion

    #region Volume Setting

    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat("MasterVolume", volume);
        _volumeTextValue.text = volume.ToString("0.0");
    }
    
    public void VolumeApply()
    {
        float volume;
        _audioMixer.GetFloat("MasterVolume", out volume);
        PlayerPrefs.SetFloat("masterVolume", volume);
        StartCoroutine(ConfirmationBox());
    }

    public IEnumerator ConfirmationBox()
    {
        _confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _confirmationPrompt.SetActive(false);
    }
    #endregion

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            _audioMixer.SetFloat("MasterVolume", _defaultVolume);
            _volumeSlider.value = _defaultVolume;
            _volumeTextValue.text = _defaultVolume.ToString("0.0");
            VolumeApply();
        }
        
        if(MenuType == "Gameplay")
        {
            _controllerSenTextValue.text = _defaultSen.ToString("0");
            _controllerSenSlider.value = _defaultSen;
            _mainControllerSen = _defaultSen;
            _invertYToggle.isOn = false;
            GameplayApply();
        }

        if (MenuType == "Graphics")
        {
            //Reset brighness value
            _brightnessSlider.value = _defaultBrightness;
            _brightnessTextValue.text = _defaultBrightness.ToString("0.0");

            _qualityDropdown.value = _defaultQualityIndex; //Medium Quality
            QualitySettings.SetQualityLevel(_defaultQualityIndex);

            _fullscreenToggle.isOn = true;
            Screen.fullScreen = true;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            _resolutionDropdown.value = _resolutions.Length;
            GraphicsApply();
        }

    }

    public async void LoadNextLevel()
    {

        await AuthService.LoginAnonymously();
        _sceneLoader.LoadSelectedLevel("Lobby");
    }
}
