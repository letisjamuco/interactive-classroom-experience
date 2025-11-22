using UnityEngine;

public class LaptopClickInteraction : MonoBehaviour
{
    [Header("Screen setup")]
    public Renderer screenRenderer;       // Screen mesh renderer
    public Material screenOnMaterial;     // Material when laptop is ON
    public Material screenOffMaterial;    // Material when laptop is OFF

    [Header("Audio")]
    public AudioSource typingAudio;       // Typing sound

    private bool isOn = false;

    void Start()
    {
        // Initialize screen state at start
        UpdateScreen();
    }

    void OnMouseDown()
    {
        // This method is called when the user LEFT-clicks the object
        // Requires a Collider on this GameObject
        isOn = !isOn;
        UpdateScreen();
    }

    void UpdateScreen()
    {
        // Toggle screen material
        if (screenRenderer != null)
            screenRenderer.material = isOn ? screenOnMaterial : screenOffMaterial;

        // Toggle typing sound
        if (typingAudio != null)
        {
            if (isOn && !typingAudio.isPlaying)
                typingAudio.Play();
            else if (!isOn && typingAudio.isPlaying)
                typingAudio.Stop();
        }
    }
}
