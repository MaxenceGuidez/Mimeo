using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    public GameObject crosshair;
    
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Pause(bool goToSettings = false)
    {
        Time.timeScale = 0;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
        
        gameObject.SetActive(!goToSettings);
        settingsMenu.gameObject.SetActive(goToSettings);
        crosshair.SetActive(false);
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        crosshair.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Time.timeScale = 1;
    }

    public void OnClickBtnResume()
    {
        Resume();
    }
    
    public void OnClickBtnSettings()
    {
        gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }
    
    public void OnClickBtnMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    
    public void OnClickBtnQuit()
    {
        Application.Quit();
    }
}
