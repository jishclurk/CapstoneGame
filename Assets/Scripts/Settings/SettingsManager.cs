using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingsManager : MonoBehaviour {

    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Canvas Menu;
    public Canvas Settings;
    public Resolution[] resolutions;
    private GameSettings gameSettings;

    void Start()
    {
        DontDestroyOnLoad(this);
        Menu = Menu.GetComponent<Canvas>();
        Settings = Settings.GetComponent<Canvas>();
        Settings.enabled = false;
    }

    private void OnEnable()
    {
        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });

        gameSettings = new GameSettings();
        gameSettings.fullscreen = fullscreenToggle.isOn = Screen.fullScreen;
        resolutionDropdown.RefreshShownValue();
        gameSettings.resolutionIndex = resolutionDropdown.value;
        gameSettings.volume = volumeSlider.value = AudioListener.volume;
    }

    public void UpdateSettingsWhenEnabled()
    {
        gameSettings.fullscreen = fullscreenToggle.isOn = Screen.fullScreen;
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
    }

    public void OnVolumeChange()
    {
        AudioListener.volume = gameSettings.volume = volumeSlider.value;
        // Debug.Log("AudioListener.volume = " + AudioListener.volume);
    }

    public void LoadSettingsMenu()
    {
        if(Menu != null)
        {
            Menu.enabled = false;
        }
        Settings.enabled = true;
    }

    public void CloseSettingsMenu()
    {
        Settings.enabled = false;
        if (Menu != null)
        {
            Menu.enabled = true;
        }
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
