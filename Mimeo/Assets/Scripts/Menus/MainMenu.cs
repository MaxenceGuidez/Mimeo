using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;

    private void Start()
    {
        settingsMenu.gameObject.SetActive(false);
    }

    public void OnClickBtnVisit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void OnClickBtnSettings()
    {
        gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    }
    
    public void OnClickBtnQuit()
    {
        Application.Quit();
    }
}
