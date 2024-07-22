using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages interactions in the main menu, including navigating to different scenes, opening the settings menu, and quitting the application.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-22</date>
public class MainMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;

    /// <summary>
    /// Handles the action when the "Visit" button is clicked.
    /// This method triggers a button click sound and loads the next scene in the build index.
    /// </summary>
    public void OnClickBtnVisit()
    {
        AudioManager.instance.OnButtonClick();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Handles the action when the "Settings" button is clicked.
    /// This method triggers a button click sound, hides the main menu, and shows the settings menu.
    /// </summary>
    public void OnClickBtnSettings()
    {
        AudioManager.instance.OnButtonClick();
        
        gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles the action when the "Quit" button is clicked.
    /// This method triggers a button click sound and quits the application.
    /// </summary>
    public void OnClickBtnQuit()
    {
        AudioManager.instance.OnButtonClick();
        
        Application.Quit();
    }
}
