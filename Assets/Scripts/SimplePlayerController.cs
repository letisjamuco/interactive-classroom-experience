using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Crouch")]
    public KeyCode crouchKey = KeyCode.C;
    public float standingHeight = 1.8f;
    public float crouchingHeight = 1.0f;
    public float crouchLerpSpeed = 8f;

    private float verticalVelocity;
    private float xRotation = 0f;

    private bool isCrouching = false;
    private float targetHeight;
    private float defaultControllerCenterY;

    private Vector3 cameraStandingLocalPos;
    private Vector3 cameraCrouchingLocalPos;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;

        // Initial height / camera settings
        // Get the current height as "standing"
        standingHeight = controller.height;
        defaultControllerCenterY = controller.center.y;

        cameraStandingLocalPos = cameraTransform.localPosition;
        cameraCrouchingLocalPos = new Vector3(
            cameraStandingLocalPos.x,
            cameraStandingLocalPos.y - 0.5f, // how much the camera will move down when crouching
            cameraStandingLocalPos.z
        );

        targetHeight = standingHeight;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleCrouch();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal"); // A/D or right/left arrow
        float z = Input.GetAxis("Vertical");   // W/S or up/down arrow

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // JUMP + GRAVITY
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        if (Input.GetButtonDown("Jump") && controller.isGrounded && !isCrouching)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        // Toggle crouch when pressing C
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = !isCrouching;
            targetHeight = isCrouching ? crouchingHeight : standingHeight;
        }

        // Smooth height change of the CharacterController
        float newHeight = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchLerpSpeed);
        controller.height = newHeight;

        // Adjust the center to stay roughly correct
        controller.center = new Vector3(
            controller.center.x,
            defaultControllerCenterY * (controller.height / standingHeight),
            controller.center.z
        );

        // Smooth change of camera position
        Vector3 targetCamPos = isCrouching ? cameraCrouchingLocalPos : cameraStandingLocalPos;
        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetCamPos,
            Time.deltaTime * crouchLerpSpeed
        );
    }
}
