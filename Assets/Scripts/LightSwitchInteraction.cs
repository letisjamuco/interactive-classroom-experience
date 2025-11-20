using UnityEngine;

public class LightSwitchInteraction : MonoBehaviour
{
    [Header("References")]
    public Light[] lightsToToggle; // lights that will turn on/off
    public Renderer bulbRenderer; // the lamp's mesh (if you have one)
    public AudioSource clickSound; // switch sound (optional)

    [Header("Settings")]
    public KeyCode interactKey = KeyCode.E;

    private bool playerNear = false;
    private bool isOn = true;

    void Start()
    {
        UpdateLights();
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(interactKey))
        {
            isOn = !isOn;
            UpdateLights();

            if (clickSound != null)
                clickSound.Play();
        }
    }

    void UpdateLights()
    {
        // enable/disable the Light components
        foreach (var l in lightsToToggle)
        {
            if (l != null) l.enabled = isOn;
        }

        // if the lamp has an emissive material
        if (bulbRenderer != null)
        {
            var mat = bulbRenderer.material;
            if (isOn)
                mat.EnableKeyword("_EMISSION");
            else
                mat.DisableKeyword("_EMISSION");
        }
    }

    // Trigger zone in front of the switch
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
