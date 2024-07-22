using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public MainMenu mainMenu;
    public PauseMenu pauseMenu;

    private void Start()
    {
        gameObject.SetActive(false);
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
}
