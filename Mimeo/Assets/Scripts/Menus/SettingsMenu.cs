using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public MainMenu mainMenu;
    public PauseMenu pauseMenu;
    
    public Slider sliderMusic;
    public Slider sliderSFX;
    public Toggle toggleFullscreen;
    public TMP_Dropdown dropdownResolutions;
    public AudioMixer audioMixer;

    private const string KEY_MUSIC = "MusicVolume";
    private const string KEY_SFX = "SFXVolume";
    private Resolution[] resolutions;

    private void Start()
    {
        gameObject.SetActive(false);
        
        audioMixer.GetFloat(KEY_MUSIC, out float musicValueForSlider);
        sliderMusic.value = musicValueForSlider;

        audioMixer.GetFloat(KEY_SFX, out float soundValueForSlider);
        sliderSFX.value = soundValueForSlider;

        InitDropdownResolutions();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(KEY_MUSIC, volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(KEY_SFX, volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void OnClickBtnReturn()
    {
        if (AudioManager.instance) AudioManager.instance.OnButtonClick();
        
        gameObject.SetActive(false);
        
        if (mainMenu) mainMenu.gameObject.SetActive(true);
        if (pauseMenu) pauseMenu.gameObject.SetActive(true);
    }

    public void OnHoverBtn()
    {
        if (AudioManager.instance) AudioManager.instance.OnButtonHover();
    }

    void InitDropdownResolutions()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        dropdownResolutions.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropdownResolutions.AddOptions(options);
        dropdownResolutions.value = currentResolutionIndex;
        dropdownResolutions.RefreshShownValue();

        Screen.fullScreen = true;
    }
}
