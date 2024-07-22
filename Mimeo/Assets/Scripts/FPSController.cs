using UnityEngine;

/// <summary>
/// Controls the first-person player movement and camera look within the game. This class manages player movement, camera orientation,
/// and sprinting functionality. It requires a `CharacterController` component to handle player movement physics.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-22</date>
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float moveSpeed = 6f;
    public float runSpeed = 12f;
    public float lookSpeed = 25f;
    public float lookXLimit = 85f;

    private CharacterController _characterController;
    private Vector3 _moveDirection = Vector3.zero;
    private float _currentSpeed;
    private float _rotationX;
    
    public static FPSController instance { get; private set; }
    
    /// <summary>
    /// Initializes the FPSController. Ensures only one instance of the controller exists and sets up initial configurations.
    /// This method is called when the script is first run.
    /// </summary>
    private void Awake()
    {
        if (instance && instance != this) Destroy(this); 
        else instance = this;
    }

    /// <summary>
    /// Sets up the `CharacterController` component, locks the cursor, hides it, and sets the initial movement speed.
    /// This method is called when the script starts.
    /// </summary>
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _currentSpeed = moveSpeed;
    }

    /// <summary>
    /// Handles player movement and camera look updates. This method is called once per frame.
    /// </summary>
    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    /// <summary>
    /// Processes player movement based on input. Moves the player in the direction of the input vector and applies the current movement speed.
    /// </summary>
    private void HandleMovement()
    {
        Vector3 inputVector = InputsManager.instance.mainInputs.FPSController.Move.ReadValue<Vector3>();
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 up = transform.TransformDirection(Vector3.up);

        _moveDirection = (forward * inputVector.z + right * inputVector.x + up * inputVector.y).normalized;
        _characterController.Move(_moveDirection * (_currentSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Updates the camera's pitch (up and down rotation) and the player's yaw (left and right rotation) based on mouse input.
    /// The camera's pitch is clamped to the specified lookXLimit to prevent excessive vertical movement.
    /// </summary>
    private void HandleLook()
    {
        Vector2 inputVector = InputsManager.instance.mainInputs.FPSController.Look.ReadValue<Vector2>();

        float mouseX = inputVector.x * lookSpeed * Time.deltaTime;
        float mouseY = inputVector.y * lookSpeed * Time.deltaTime;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Updates the player's movement speed based on whether the player is sprinting or not.
    /// </summary>
    /// <param name="isSprinting">Indicates whether the player is currently sprinting.</param>
    public void Sprint(bool isSprinting)
    {
        _currentSpeed = isSprinting ? runSpeed : moveSpeed;
    }
}
