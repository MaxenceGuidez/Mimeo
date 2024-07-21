using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    public GameObject crosshair;
    public PanelInfos panelInfos;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Pause(bool goToSettings = false)
    {
        EventSystem.current.SetSelectedGameObject(null);
        InputsManager.instance.mainInputs.Selector.Disable();

        panelInfos.CloseDirectly();
        Selector.instance.Unselect();
        Selector.instance.UnhighlightObject();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        gameObject.SetActive(!goToSettings);
        settingsMenu.gameObject.SetActive(goToSettings);
        crosshair.SetActive(false);
        Time.timeScale = 0;
    }

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
