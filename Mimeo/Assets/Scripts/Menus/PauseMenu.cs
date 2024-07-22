using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the pause menu functionality, including pausing the game, resuming gameplay, and navigating to settings or the main menu.
/// This class handles UI interactions related to pausing the game, such as showing/hiding the pause menu and managing game timescale.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-22</date>
public class PauseMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    public GameObject crosshair;
    public PanelInfos panelInfos;

    /// <summary>
    /// Initializes the pause menu by hiding it at the start.
    /// This method ensures the pause menu is not visible when the game starts.
    /// </summary>
    private void Start()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Pauses the game and shows the pause menu.
    /// This method also optionally navigates to the settings menu and disables player input.
    /// </summary>
    /// <param name="goToSettings">Indicates whether to navigate to the settings menu.</param>
    public void Pause(bool goToSettings = false)
    {
        EventSystem.current.SetSelectedGameObject(null);
        InputsManager.instance.mainInputs.Selector.Disable();

        panelInfos.CloseDirectly();
        Selector.instance.Unselect();
        Selector.instance.Unhighlight();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        gameObject.SetActive(!goToSettings);
        settingsMenu.gameObject.SetActive(goToSettings);
        crosshair.SetActive(false);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resumes the game and hides the pause menu.
    /// This method restores the game timescale, re-enables player input, and shows the crosshair.
    /// </summary>
    public void Resume()
    {
        gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        crosshair.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputsManager.instance.mainInputs.Selector.Enable();
        
        Time.timeScale = 1;
    }

    /// <summary>
    /// Handles the action when the "Resume" button is clicked.
    /// This method plays a button click sound and resumes the game.
    /// </summary>
    public void OnClickBtnResume()
    {
        if (AudioManager.instance) AudioManager.instance.OnButtonClick();
        
        Resume();
    }
    
    /// <summary>
    /// Handles the action when the "Settings" button is clicked.
    /// This method plays a button click sound, hides the pause menu, and shows the settings menu.
    /// </summary>
    public void OnClickBtnSettings()
    {
        if (AudioManager.instance) AudioManager.instance.OnButtonClick();
        
        gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Handles the action when the "Main Menu" button is clicked.
    /// This method plays a button click sound, resets the game time scale, and loads the main menu scene.
    /// </summary>
    public void OnClickBtnMainMenu()
    {
        if (AudioManager.instance) AudioManager.instance.OnButtonClick();
        
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    
    /// <summary>
    /// Handles the action when the "Quit" button is clicked.
    /// This method plays a button click sound and quits the application.
    /// </summary>
    public void OnClickBtnQuit()
    {
        if (AudioManager.instance) AudioManager.instance.OnButtonClick();
        
        Application.Quit();
    }

    /// <summary>
    /// Handles the action when a button is hovered over.
    /// This method plays a hover sound effect.
    /// </summary>
    public void OnHoverBtn()
    {
        if (AudioManager.instance) AudioManager.instance.OnButtonHover();
    }
}
