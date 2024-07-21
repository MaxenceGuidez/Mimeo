using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public MainInputs mainInputs;
    
    public static InputsManager instance { get; private set; }
    
    private void Awake()
    {
        if (instance && instance != this)  Destroy(this); 
        else instance = this;
        
        mainInputs = new MainInputs();
    }
    
    private void OnEnable()
    {
        mainInputs.FPSController.Enable();
        mainInputs.Selector.Enable();
        mainInputs.Menu.Enable();
        
        mainInputs.FPSController.Sprint.performed += Sprint;
        mainInputs.FPSController.Sprint.canceled += SprintCanceled;
        mainInputs.Selector.Select.performed += Select;
        mainInputs.Selector.Unselect.performed += Unselect;
        mainInputs.Menu.Pause.performed += Pause;
        mainInputs.Menu.Settings.performed += OpenSettings;
    }

    private void OnDisable()
    {
        mainInputs.FPSController.Disable();
        mainInputs.Selector.Disable();
        mainInputs.Menu.Disable();
        
        mainInputs.FPSController.Sprint.performed -= Sprint;
        mainInputs.FPSController.Sprint.canceled -= SprintCanceled;
        mainInputs.Selector.Select.performed -= Select;
        mainInputs.Selector.Unselect.performed -= Unselect;
        mainInputs.Menu.Pause.performed -= Pause;
        mainInputs.Menu.Settings.performed -= OpenSettings;
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        FPSController.instance.Sprint(true);
    }

    private void SprintCanceled(InputAction.CallbackContext context)
    {
        FPSController.instance.Sprint(false);
    }

    private void Select(InputAction.CallbackContext context)
    {
        Selector.instance.Select();
    }

    private void Unselect(InputAction.CallbackContext context)
    {
        Selector.instance.Unselect();
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
