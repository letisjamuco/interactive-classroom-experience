using UnityEngine;
using UnityEngine.EventSystems;

public class SimplePlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Crouch")]
    public KeyCode crouchKey = KeyCode.C;
    public float standingHeight = 1.8f;
    public float crouchingHeight = 1.0f;
    public float crouchLerpSpeed = 4f;

    [Header("Look")]
    [Tooltip("If false, mouse look is disabled (used when typing in the notebook).")]
    public bool canLook = true;

    [Header("Notebook")]
    [Tooltip("Set to true while the notebook UI is open to disable WASD / crouch.")]
    public bool notebookOpen = false;

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
        Cursor.visible = false;

        // Use the current controller height as standing height
        standingHeight = controller.height;
        defaultControllerCenterY = controller.center.y;

        // Store camera positions for standing / crouching
        cameraStandingLocalPos = cameraTransform.localPosition;
        cameraCrouchingLocalPos = new Vector3(
            cameraStandingLocalPos.x,
            cameraStandingLocalPos.y - 0.5f,
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

    // Check if user is typing in a UI input field
    private bool IsTypingInUI()
    {
        // Check if any UI element is selected (like an InputField)
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem != null && eventSystem.currentSelectedGameObject != null)
        {
            // Check if the selected object is an InputField
            UnityEngine.UI.InputField inputField = eventSystem.currentSelectedGameObject.GetComponent<UnityEngine.UI.InputField>();
            TMPro.TMP_InputField tmpInputField = eventSystem.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>();

            return (inputField != null || tmpInputField != null);
        }
        return false;
    }

    void HandleMovement()
    {
        float x = 0f;
        float z = 0f;

        // If user is typing in UI, don't process movement keys EXCEPT arrow keys
        bool typing = IsTypingInUI();

        if (typing)
        {
            // When typing: ONLY arrow keys work for movement
            if (Input.GetKey(KeyCode.LeftArrow)) x = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) x = 1f;
            if (Input.GetKey(KeyCode.UpArrow)) z = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) z = -1f;
        }
        else if (notebookOpen)
        {
            // Notebook open but NOT typing: Arrow keys only
            if (Input.GetKey(KeyCode.LeftArrow)) x = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) x = 1f;
            if (Input.GetKey(KeyCode.UpArrow)) z = 1f;
            if (Input.GetKey(KeyCode.DownArrow)) z = -1f;
        }
        else
        {
            // Normal mode: both WASD and arrows work
            x = Input.GetAxis("Horizontal"); // A/D or left/right arrow
            z = Input.GetAxis("Vertical");   // W/S or up/down arrow
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Jump + gravity (disabled when notebook is open OR typing)
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        if (!notebookOpen && !typing && Input.GetButtonDown("Jump") && controller.isGrounded && !isCrouching)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        // Mouse look works ALWAYS (even when notebook is open), unless canLook is false
        if (!canLook)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        // CRITICAL: Crouch is disabled when notebook is open OR when typing in UI
        if (notebookOpen || IsTypingInUI())
        {
            return;
        }

        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = !isCrouching;
            targetHeight = isCrouching ? crouchingHeight : standingHeight;
        }

        // Smooth height change of the CharacterController
        float newHeight = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchLerpSpeed);
        controller.height = newHeight;

        // Adjust center so the capsule stays on the floor
        controller.center = new Vector3(
            controller.center.x,
            defaultControllerCenterY * (controller.height / standingHeight),
            controller.center.z
        );

        // Smooth camera position change
        Vector3 targetCamPos = isCrouching ? cameraCrouchingLocalPos : cameraStandingLocalPos;
        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetCamPos,
            Time.deltaTime * crouchLerpSpeed
        );
    }
}