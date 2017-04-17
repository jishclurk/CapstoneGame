using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingsManager : MonoBehaviour {

    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Canvas Settings;
    public Resolution[] resolutions;
    private GameSettings gameSettings;

    void Start()
    {
        Settings = Settings.GetComponent<Canvas>();
        Settings.enabled = false;
    }

    private void OnEnable()
    {
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });

        gameSettings = new GameSettings();
        gameSettings.fullscreen = fullscreenToggle.isOn = Screen.fullScreen;
        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
        Resolution current = Screen.currentResolution;
        Debug.Log("current = " + current.ToString());
        resolutionDropdown.RefreshShownValue();
        gameSettings.resolutionIndex = resolutionDropdown.value;
        gameSettings.volume = volumeSlider.value = AudioListener.volume;
    }

    public void UpdateSettingsWhenEnabled()
    {
        gameSettings.fullscreen = fullscreenToggle.isOn = Screen.fullScreen;
        string currentResolutionWidth = Screen.width.ToString();
        string currentResolutionHeight = Screen.height.ToString();
        foreach(Dropdown.OptionData option in resolutionDropdown.options)
        {
            if (option.text.ToString().Contains(currentResolutionWidth)
                && option.text.ToString().Contains(currentResolutionHeight))
            {
                resolutionDropdown.value = resolutionDropdown.options.IndexOf(option);
            }
        }
        resolutionDropdown.RefreshShownValue();
        gameSettings.resolutionIndex = resolutionDropdown.value;
        gameSettings.volume = volumeSlider.value = AudioListener.volume;
    }

    public void OnFullscreenToggle()
    {
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width,
            resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void OnVolumeChange()
    {
        AudioListener.volume = gameSettings.volume = volumeSlider.value;
    }

    public void LoadSettingsMenu()
    {
        Settings.enabled = true;
    }

    public void CloseSettingsMenu()
    {
        Settings.enabled = false;
    }

    /*
    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(Application.persistentDataPath + "/gamesettings.json");
        volumeSlider.value = gameSettings.volume;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        fullscreenToggle.isOn = gameSettings.fullscreen;
    }*/
}
