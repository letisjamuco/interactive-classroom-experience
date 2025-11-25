using UnityEngine;

public class LightSwitchInteraction : MonoBehaviour
{
    [Header("References")]
    public Light[] lightsToToggle;
    public MeshRenderer bulbRenderer;
    public AudioSource clickSound;

    [Header("Settings")]
    public KeyCode interactKey = KeyCode.E;

    private bool isPlayerInside = false;
    private bool lightsOn = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            InteractionHintUI.Instance?.ShowHint("Press E to toggle lights.\n Try to press N for full night experience!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            InteractionHintUI.Instance?.HideHint();
        }
    }

    private void Update()
    {
        if (!isPlayerInside) return;

        if (Input.GetKeyDown(interactKey))
        {
            ToggleLights();
        }
    }

    private void ToggleLights()
    {
        lightsOn = !lightsOn;

        foreach (var l in lightsToToggle)
        {
            if (l != null) l.enabled = lightsOn;
        }

        if (bulbRenderer != null)
        {
            if (lightsOn)
                bulbRenderer.material.EnableKeyword("_EMISSION");
            else
                bulbRenderer.material.DisableKeyword("_EMISSION");
        }

        if (clickSound != null)
        {
            clickSound.Play();
        }
    }
}