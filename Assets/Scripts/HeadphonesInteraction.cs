using UnityEngine;

public class HeadphonesInteraction : MonoBehaviour
{
    public AudioSource music;
    private bool inside = false;
    private bool isPlaying = false;

    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        if (!inside) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (!isPlaying)
                music.Play();
            else
                music.Stop();

            isPlaying = !isPlaying;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
            InteractionHintUI.Instance?.ShowHint("Press E to play/pause music");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
            InteractionHintUI.Instance?.HideHint();
        }
    }
}
