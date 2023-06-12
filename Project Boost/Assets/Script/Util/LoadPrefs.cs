using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool _canUse = false;
    [SerializeField] private MenuController _menuController;

    [Header("Volume Setting")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TMP_Text _volumeTextValue = null;
    [SerializeField] private Slider _volumeSlider = null;

    [Header("Brightness Setting")]
    [SerializeField] private Slider _brightnessSlider = null;
    [SerializeField] private TMP_Text _brightnessTextValue = null;

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle _fullscreenToggle;

    [Header("Sentivity Setting")]
    [SerializeField] private TMP_Text _controllerSenTextValue = null;
    [SerializeField] private Slider _controllerSenSlider = null;

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle _invertToggle = null;

    private void Awake()
    {
        if (_canUse)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                _volumeTextValue.text = localVolume.ToString("0.0");
                _volumeSlider.value = localVolume;
                _audioMixer.SetFloat("MasterVolume", localVolume);
            }
            else
            {
                _menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                _qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("masterFullScreen"))
            {
                int localFullScreen = PlayerPrefs.GetInt("masterFullScreen");

                if(localFullScreen == 1)
                {
                    Screen.fullScreen = true;
                    _fullscreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    _fullscreenToggle.isOn = false;
                }
            }

            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                _brightnessTextValue.text = localBrightness.ToString("0.0");
                _brightnessSlider.value = localBrightness;
                //Change brightness
            }

            if (PlayerPrefs.HasKey("masterSen"))
            {
                float localSentivity = PlayerPrefs.GetFloat("masterSen");

                _controllerSenTextValue.text = localSentivity.ToString("0.0");
                _controllerSenSlider.value = localSentivity;
                _menuController._mainControllerSen = Mathf.RoundToInt(localSentivity);
            }

            if (PlayerPrefs.HasKey("masterInvertY"))
            {
                if(PlayerPrefs.GetInt("masterInvertY") == 1)
                {
                    _invertToggle.isOn = true;
                }
                else 
                {
                    _invertToggle.isOn = false;
                }
            }

        }
    }

}
