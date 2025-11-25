using UnityEngine;

public class BookInfoInteraction : MonoBehaviour
{
    [Header("References")]
    public GameObject bookCanvas;
    public SimplePlayerController playerController;

    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode closeKey = KeyCode.Escape;

    private bool playerInside = false;
    private bool isOpen = false;

    void Start()
    {
        if (bookCanvas != null)
        {
            bookCanvas.SetActive(false);
            Debug.Log("Book canvas initialized - hidden");
        }
        else
        {
            Debug.LogError("BookCanvas is NOT assigned! Please assign it in Inspector.");
        }
    }

    void Update()
    {
        if (isOpen && Input.GetKeyDown(closeKey))
        {
            CloseBookPanel();
            return;
        }

        if (!playerInside) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (!isOpen)
                OpenBookPanel();
            else
                CloseBookPanel();
        }
    }

    private void OpenBookPanel()
    {
        if (bookCanvas == null)
        {
            Debug.LogError("Cannot open book - Canvas not assigned!");
            return;
        }

        bookCanvas.SetActive(true);
        isOpen = true;
        Debug.Log("Book opened!");
    }

    private void CloseBookPanel()
    {
        if (bookCanvas != null)
        {
            bookCanvas.SetActive(false);
        }

        isOpen = false;
        Debug.Log("Book closed!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            InteractionHintUI.Instance?.ShowHint("Press E to open book.\nMove away or press ESC to close.");
            Debug.Log("Player entered book zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            InteractionHintUI.Instance?.HideHint();

            if (isOpen)
            {
                CloseBookPanel();
            }

            Debug.Log("Player left book zone");
        }
    }
}