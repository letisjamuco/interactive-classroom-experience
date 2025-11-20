using UnityEngine;

public class NotebookInteraction : MonoBehaviour
{
    public GameObject notePanel;
    public KeyCode interactKey = KeyCode.E;

    private bool playerNear = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(interactKey))
        {
            bool newState = !notePanel.activeSelf;
            notePanel.SetActive(newState);

            // when opening, unlock the cursor; optional
            Cursor.lockState = newState ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // close with ESC
        if (notePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            notePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
