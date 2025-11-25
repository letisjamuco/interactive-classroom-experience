using UnityEngine;

public class HeadphonesInteraction : MonoBehaviour
{
    [Header("References")]
    public AudioSource music;

    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;

    private bool playerInside = false;
    private bool isPlaying = false;

    void Start()
    {
        if (music != null)
        {
            music.Stop();
            isPlaying = false;
        }
    }

    void Update()
    {
        if (!playerInside) return;

        if (Input.GetKeyDown(interactKey))
        {
            ToggleMusic();
        }
    }

    private void ToggleMusic()
    {
        if (music == null) return;

        if (!isPlaying)
        {
            music.Play();
            isPlaying = true;
            Debug.Log("Music started");
        }
        else
        {
            music.Stop();
            isPlaying = false;
            Debug.Log("Music stopped");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            InteractionHintUI.Instance?.ShowHint("Press E to play/pause music");
            Debug.Log("Player entered headphones zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            InteractionHintUI.Instance?.HideHint();

            if (isPlaying && music != null)
            {
                music.Stop();
                isPlaying = false;
                Debug.Log("Music stopped - player left");
            }
        }
    }
}