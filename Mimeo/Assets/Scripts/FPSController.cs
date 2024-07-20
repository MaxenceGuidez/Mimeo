using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float moveSpeed = 6f;
    public float runSpeed = 12f;
    public float lookSpeed = 25f;
    public float lookXLimit = 85f;

    private CharacterController _characterController;
    private MainInputs _mainInputs;
    private Vector3 _moveDirection = Vector3.zero;
    private float _currentSpeed;
    private float _rotationX;

    private void Awake()
    {
        _mainInputs = new MainInputs();
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _currentSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        _mainInputs.FPSController.Enable();
        _mainInputs.FPSController.Sprint.performed += Sprint;
        _mainInputs.FPSController.Sprint.canceled += SprintCanceled;
    }

    private void OnDisable()
    {
        _mainInputs.FPSController.Disable();
        _mainInputs.FPSController.Sprint.performed -= Sprint;
        _mainInputs.FPSController.Sprint.canceled -= SprintCanceled;
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        Vector3 inputVector = _mainInputs.FPSController.Move.ReadValue<Vector3>();
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 up = transform.TransformDirection(Vector3.up);

        _moveDirection = (forward * inputVector.z + right * inputVector.x + up * inputVector.y).normalized;
        _characterController.Move(_moveDirection * (_currentSpeed * Time.deltaTime));
    }

    private void HandleLook()
    {
        Vector2 inputVector = _mainInputs.FPSController.Look.ReadValue<Vector2>();

        float mouseX = inputVector.x * lookSpeed * Time.deltaTime;
        float mouseY = inputVector.y * lookSpeed * Time.deltaTime;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        _currentSpeed = runSpeed;
    }

    private void SprintCanceled(InputAction.CallbackContext context)
    {
        _currentSpeed = moveSpeed;
    }
}
