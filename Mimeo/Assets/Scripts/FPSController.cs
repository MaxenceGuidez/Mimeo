using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float moveSpeed = 6f;
    public float runSpeed = 12f;
    public float lookSpeed = 2f;
    public float lookXLimit = 85f;
    public bool canMove = true;

    CharacterController _characterController;
    Vector3 _moveDirection = Vector3.zero;
    float _rotationX = 0;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 up = transform.TransformDirection(Vector3.up);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeed = isRunning ? runSpeed : moveSpeed;

        float curSpeedX = canMove ? curSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? curSpeed * Input.GetAxis("Horizontal") : 0;
        float curSpeedZ = 0;

        if (canMove)
        {
            if (Input.GetKey(KeyCode.E))
            {
                curSpeedZ = curSpeed;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                curSpeedZ = -curSpeed;
            }
        }

        _moveDirection = (forward * curSpeedX) + (right * curSpeedY) + (up * curSpeedZ);
        _characterController.Move(_moveDirection * Time.deltaTime);
        #endregion

        #region Handles Rotation
        if (canMove)
        {
            _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion
    }
}
