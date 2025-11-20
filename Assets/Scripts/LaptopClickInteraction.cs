using UnityEngine;

public class LaptopClickInteraction : MonoBehaviour
{
    public Renderer screenRenderer;    // only the screen mesh
    public Material screenOnMaterial;  // material for "screen on"
    public Material screenOffMaterial; // material for "screen off"

    public AudioSource typingAudio;    // typing sound (loop or one-shot)

    private bool isOn = false;

    void Start()
    {
        UpdateScreen();
    }

    void OnMouseDown()
    {
        // requires collider on the laptop/screen
        isOn = !isOn;
        UpdateScreen();
    }

    void UpdateScreen()
    {
        if (screenRenderer != null)
        {
            screenRenderer.material = isOn ? screenOnMaterial : screenOffMaterial;
        }

        if (typingAudio != null)
        {
            if (isOn && !typingAudio.isPlaying)
                typingAudio.Play();
            else if (!isOn && typingAudio.isPlaying)
                typingAudio.Stop();
        }
    }
}
