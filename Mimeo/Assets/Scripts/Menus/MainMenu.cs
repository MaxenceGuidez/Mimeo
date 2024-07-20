using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;

    private void Start()
    {
        settingsMenu.SetActive(false);
    }

    public void OnClickBtnVisit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void OnClickBtnSettings()
    {
        gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }
    
    public void OnClickBtnQuit()
    {
        Application.Quit();
    }
}
