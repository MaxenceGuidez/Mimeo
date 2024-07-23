using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages input actions for the application, handling various game and UI interactions.
/// This class follows the Singleton design pattern to ensure a single instance exists throughout the application.
/// It integrates with the Unity Input System to process inputs for FPS controls, menu interactions, and selection actions.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-23</date>
public class InputsManager : MonoBehaviour
{
    public PauseMenu pauseMenu;
    public MainInputs mainInputs;
    
    private float _oldLookSpeed;
    
    public static InputsManager instance { get; private set; }
    
    /// <summary>
    /// Initializes the InputsManager instance and sets up input actions.
    /// If another instance of InputsManager exists, it destroys the new one to maintain a single instance.
    /// </summary>
    private void Awake()
    {
        if (instance && instance != this) Destroy(this); 
        else instance = this;
        
        mainInputs = new MainInputs();
    }
    
    /// <summary>
    /// This method is called when the script is enabled, setting up the input action callbacks.
    /// </summary>
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

    /// <summary>
    /// This method is called when the script is disabled, removing the input action callbacks.
    /// </summary>
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

    /// <summary>
    /// Enables the selection mode by adjusting cursor visibility and look speed.
    /// This method unlocks the cursor and changes the FPS controller's look speed for selection purposes.
    /// </summary>
    public void EnableSelectionMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        mainInputs.FPSController.Move.Disable();

        float actualLookSpeed = FPSController.instance.lookSpeed;
        if (!Mathf.Approximately(actualLookSpeed, 2f)) _oldLookSpeed = actualLookSpeed;
        FPSController.instance.lookSpeed = 2f;
    }

    /// <summary>
    /// Disables the selection mode by restoring cursor lock state and look speed.
    /// This method re-enables the FPS controller's movement and resets the look speed to its original value.
    /// </summary>
    public void DisableSelectionMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        mainInputs.FPSController.Move.Enable();
        FPSController.instance.lookSpeed = _oldLookSpeed;
    }

    /// <summary>
    /// Handles the sprint input action, enabling sprinting in the FPS controller.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void Sprint(InputAction.CallbackContext context)
    {
        FPSController.instance.Sprint(true);
    }

    /// <summary>
    /// Handles the sprint canceled input action, disabling sprinting in the FPS controller.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void SprintCanceled(InputAction.CallbackContext context)
    {
        FPSController.instance.Sprint(false);
    }

    /// <summary>
    /// Handles the sprint canceled input action, disabling sprinting in the FPS controller.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void Select(InputAction.CallbackContext context)
    {
        Selector.instance.Select();
    }

    /// <summary>
    /// Handles the unselect input action, triggering the unselection action in the Selector.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void Unselect(InputAction.CallbackContext context)
    {
        Selector.instance.Unselect();
    }

    /// <summary>
    /// Handles the pause input action, triggering the pause functionality in the PauseMenu. If the app is already paused,
    /// it resumes.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void Pause(InputAction.CallbackContext context)
    {
        if (Time.timeScale != 0)
        {
            pauseMenu.Pause();
        }
        else
        {
            pauseMenu.Resume();
        }
    }

    /// <summary>
    /// Handles the settings input action, opening the settings menu through the PauseMenu.
    /// </summary>
    /// <param name="context">The input action callback context.</param>
    private void OpenSettings(InputAction.CallbackContext context)
    {
        pauseMenu.Pause(true);
    }
}
