using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public void OnClickBtnReturn()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
