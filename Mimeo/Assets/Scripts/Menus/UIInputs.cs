using UnityEngine;
using UnityEngine.InputSystem;

public class InGameInputs : MonoBehaviour
{
    public PauseMenu pauseMenu;
    
    private MainInputs _mainInputs;

    private void Awake()
    {
        _mainInputs = new MainInputs();
    }

    private void OnEnable()
    {
        _mainInputs.Menu.Enable();
        _mainInputs.Menu.Pause.performed += Pause;
        _mainInputs.Menu.Settings.performed += OpenSettings;
    }

    private void OnDisable()
    {
        _mainInputs.Menu.Disable();
        _mainInputs.Menu.Pause.performed -= Pause;
        _mainInputs.Menu.Settings.performed -= OpenSettings;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        pauseMenu.Pause();
    }

    private void OpenSettings(InputAction.CallbackContext context)
    {
        pauseMenu.Pause(true);
    }
}
