using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;

    public void OnClickBtnVisit()
    {
        AudioManager.instance.OnButtonClick();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnClickBtnSettings()
    {
        AudioManager.instance.OnButtonClick();
        
        gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }

    public void OnClickBtnQuit()
    {
        AudioManager.instance.OnButtonClick();
        
        Application.Quit();
    }
}
