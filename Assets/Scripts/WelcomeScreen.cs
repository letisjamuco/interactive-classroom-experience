using UnityEngine;

public class WelcomeScreen : MonoBehaviour
{
    [Header("References")]
    public GameObject welcomeCanvas;           // The UI Canvas with instructions
    public SimplePlayerController playerController; // Reference to player controller

    [Header("Input")]
    public KeyCode continueKey = KeyCode.Space; // Key to close welcome screen (Space or Enter)
    public KeyCode alternativeKey = KeyCode.Return;

    private bool isShowing = true;

    [System.Obsolete]
    void Start()
    {
        // Show welcome screen at start
        if (welcomeCanvas != null)
        {
            welcomeCanvas.SetActive(true);
        }

        // Find player controller if not set
        if (playerController == null)
        {
            playerController = FindObjectOfType<SimplePlayerController>();
        }

        // Disable player movement while showing welcome screen
        if (playerController != null)
        {
            playerController.notebookOpen = true; // Use this flag to disable movement
            playerController.canLook = false;     // Disable camera movement
        }

        // Show cursor for clicking
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Welcome screen shown. Press SPACE or ENTER to start.");
    }

    void Update()
    {
        if (!isShowing)
            return;

        // Check for key press to close welcome screen
        if (Input.GetKeyDown(continueKey) || Input.GetKeyDown(alternativeKey))
        {
            CloseWelcomeScreen();
        }
    }

    // This can also be called from a UI Button
    public void CloseWelcomeScreen()
    {
        if (!isShowing)
            return;

        isShowing = false;

        // Hide welcome canvas
        if (welcomeCanvas != null)
        {
            welcomeCanvas.SetActive(false);
        }

        // Re-enable player movement
        if (playerController != null)
        {
            playerController.notebookOpen = false;
            playerController.canLook = true;
        }

        // Lock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Welcome screen closed. Game started!");
    }
}