using UnityEngine;

public class NotebookInteraction : MonoBehaviour
{
    [Header("References")]
    public GameObject notebookCanvas;               // World-space Canvas object
    public SimplePlayerController playerController; // Player controller

    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;         // Open notebook
    public KeyCode closeKey = KeyCode.Escape;       // Close notebook

    private bool playerInside = false;
    private bool isOpen = false;

    public InteractableHighlight highlight;

    void Start()
    {
        if (notebookCanvas != null)
            notebookCanvas.SetActive(false);        // Hidden at start
    }

    void Update()
    {
        // Check for close key even if player is outside
        if (isOpen && Input.GetKeyDown(closeKey))
        {
            CloseNotebook();
            return;
        }

        // Only allow opening when inside trigger
        if (!playerInside)
            return;

        // Open with E
        if (!isOpen && Input.GetKeyDown(interactKey))
        {
            OpenNotebook();
        }
    }

    private void OpenNotebook()
    {
        if (notebookCanvas != null)
            notebookCanvas.SetActive(true);

        isOpen = true;

        // Set notebookOpen flag (this disables WASD, crouch, but NOT arrows or mouse look)
        if (playerController != null)
        {
            playerController.notebookOpen = true;
            // Keep canLook = true so mouse look still works!
        }

        // Unlock cursor so user can click on UI and type
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Notebook opened - Arrow keys and mouse look still work!");
    }

    private void CloseNotebook()
    {
        if (notebookCanvas != null)
            notebookCanvas.SetActive(false);

        isOpen = false;

        // Clear notebookOpen flag
        if (playerController != null)
        {
            playerController.notebookOpen = false;
        }

        // CRITICAL: Deselect any UI element (like InputField) when closing
        UnityEngine.EventSystems.EventSystem eventSystem = UnityEngine.EventSystems.EventSystem.current;
        if (eventSystem != null)
        {
            eventSystem.SetSelectedGameObject(null);
        }

        // Lock cursor back for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Notebook closed and UI deselected");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            highlight?.SetHighlighted(true);
            InteractionHintUI.Instance?.ShowHint("Press E to open notebook. \n Move away to close notebook or press ESC.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            highlight?.SetHighlighted(false);
            InteractionHintUI.Instance?.HideHint();

            if (isOpen)
                CloseNotebook();
        }
    }
}