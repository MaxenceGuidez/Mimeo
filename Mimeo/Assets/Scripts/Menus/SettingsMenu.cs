using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Manages the settings menu functionality, allowing the user to adjust music and sound effects volumes, toggle fullscreen mode,
/// and change screen resolution. This class interacts with Unity's UI elements and audio system to apply user preferences.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-22</date>
public class SettingsMenu : MonoBehaviour
{
    public MainMenu mainMenu;
    public PauseMenu pauseMenu;
    
    public Slider sliderMusic;
    public Slider sliderSFX;
    public TMP_Dropdown dropdownResolutions;
    public AudioMixer audioMixer;

    private const string KEY_MUSIC = "MusicVolume";
    private const string KEY_SFX = "SFXVolume";
    private Resolution[] resolutions;

    /// <summary>
    /// Initializes the settings menu, setting the default values for the audio sliders and resolution dropdown.
    /// This method is called when the script is first run.
    /// </summary>
    private void Start()
    {
        gameObject.SetActive(false);
        
        audioMixer.GetFloat(KEY_MUSIC, out float musicValueForSlider);
        sliderMusic.value = musicValueForSlider;

        audioMixer.GetFloat(KEY_SFX, out float soundValueForSlider);
        sliderSFX.value = soundValueForSlider;

        InitDropdownResolutions();
    }

    /// <summary>
    /// Sets the volume of the music.
    /// This method updates the music volume in the audio mixer based on the slider value.
    /// </summary>
    /// <param name="volume">The volume level to set, ranging from -80 to 0 dB.</param>
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(KEY_MUSIC, volume);
    }

    /// <summary>
    /// Sets the volume of the sound effects.
    /// This method updates the sound effects volume in the audio mixer based on the slider value.
    /// </summary>
    /// <param name="volume">The volume level to set, ranging from -80 to 0 dB.</param>
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(KEY_SFX, volume);
    }

    /// <summary>
    /// Toggles fullscreen mode on or off.
    /// This method updates the screen mode based on the toggle value.
    /// </summary>
    /// <param name="isFullScreen">Indicates whether to enable fullscreen mode.</param>
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    /// <summary>
    /// Changes the screen resolution based on the selected index from the dropdown menu.
    /// This method updates the screen resolution to match the selected option.
    /// </summary>
    /// <param name="resolutionIndex">The index of the selected resolution from the dropdown menu.</param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    /// <summary>
    /// Handles the action when the "Return" button is clicked.
    /// This method plays a button click sound, hides the settings menu, and shows the main menu or pause menu.
    /// </summary>
    public void OnClickBtnReturn()
    {
        if (AudioManager.instance) AudioManager.instance.OnButtonClick();
        
        gameObject.SetActive(false);
        
        if (mainMenu) mainMenu.gameObject.SetActive(true);
        if (pauseMenu) pauseMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles the action when a button is hovered over.
    /// This method plays a hover sound effect.
    /// </summary>
    public void OnHoverBtn()
    {
        if (AudioManager.instance) AudioManager.instance.OnButtonHover();
    }

    /// <summary>
    /// Initializes the dropdown menu for screen resolutions.
    /// This method populates the dropdown with available resolutions and sets the current resolution as the default option.
    /// </summary>
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
