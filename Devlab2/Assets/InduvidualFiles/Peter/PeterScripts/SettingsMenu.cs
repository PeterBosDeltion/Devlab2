using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public bool inSettings;
    public GameObject settingsMenu;

    public GameObject mainMenu;

    public Camera mainCam;
    public Camera settingsCam;
    public Camera startCam;

    Resolution[] resolutions;
    private void Start()
    {
        LoadResolutions();
        mainCam = Camera.main;
        settingsCam.enabled = false;
        startCam.enabled = false;
    }

    public void LoadSettings()
    {
        if (!inSettings)
        {
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
            mainCam.enabled = false;
            settingsCam.enabled = true;
            inSettings = true;
        }
        else
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
            settingsCam.enabled = false;
            mainCam.enabled = true;
            inSettings = false;
        }
    }

    private void LoadResolutions()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResoltion(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
