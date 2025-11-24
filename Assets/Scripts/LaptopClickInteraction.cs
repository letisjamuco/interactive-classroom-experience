using UnityEngine;

public class LaptopClickInteraction : MonoBehaviour
{
    [Header("Screen visuals")]
    public Renderer screenRenderer;       // Renderer of the laptop screen
    public int screenMaterialIndex = 2;   // Which material slot is the actual screen
    public Material screenOnMaterial;     // Material when screen is ON
    public Material screenOffMaterial;    // Material when screen is OFF

    [Header("Audio")]
    public AudioSource typingAudio;       // Keyboard typing sound

    [Header("Interaction")]
    public float maxClickDistance = 3f;   // Max distance from camera
    public LayerMask raycastLayers = ~0;  // Layers raycast can hit

    private bool isOn = false;
    private bool playerInside = false;

    void Start()
    {
        UpdateScreenAndAudio();
        Debug.Log("LaptopClickInteraction initialized on: " + gameObject.name);
    }

    void Update()
    {
        // Left mouse button - only when player is inside trigger
        if (playerInside && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse clicked while inside laptop trigger");
            TryClickLaptop();
        }
    }

    private void TryClickLaptop()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("No main camera found!");
            return;
        }

        // Raycast from center of screen
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        // Debug: Draw the ray in scene view
        Debug.DrawRay(ray.origin, ray.direction * maxClickDistance, Color.red, 2f);

        // Simplified: If player is inside trigger and looking somewhat towards laptop, toggle it
        // Check if raycast hits ANYTHING (including this laptop's parts)
        RaycastHit[] allHits = Physics.RaycastAll(ray, maxClickDistance, raycastLayers);

        bool hitThisLaptop = false;
        foreach (RaycastHit h in allHits)
        {
            // Check if this hit is part of our laptop hierarchy
            if (IsPartOfThisLaptop(h.collider.gameObject))
            {
                hitThisLaptop = true;
                Debug.Log("Raycast HIT laptop part: " + h.collider.gameObject.name);
                break;
            }
        }

        if (hitThisLaptop)
        {
            ToggleLaptop();
        }
        else
        {
            // Even if raycast doesn't hit laptop, if player is close and inside trigger, allow toggle
            // This makes it more forgiving
            Debug.Log("Didn't hit laptop directly, but player is in trigger zone - toggling anyway");
            ToggleLaptop();
        }
    }

    private bool IsPartOfThisLaptop(GameObject hitObject)
    {
        // Check if the hit object is this GameObject or any of its children
        Transform current = hitObject.transform;
        Transform laptopTransform = this.transform;

        while (current != null)
        {
            if (current == laptopTransform)
                return true;
            current = current.parent;
        }

        return false;
    }

    private void ToggleLaptop()
    {
        isOn = !isOn;
        Debug.Log("Laptop toggled: " + (isOn ? "ON" : "OFF"));
        UpdateScreenAndAudio();
    }

    private void UpdateScreenAndAudio()
    {
        // Update screen material
        if (screenRenderer != null)
        {
            Material[] mats = screenRenderer.materials;
            if (screenMaterialIndex >= 0 && screenMaterialIndex < mats.Length)
            {
                mats[screenMaterialIndex] = isOn ? screenOnMaterial : screenOffMaterial;
                screenRenderer.materials = mats;
                Debug.Log("Screen material updated to: " + (isOn ? "ON" : "OFF"));
            }
            else
            {
                Debug.LogWarning("Screen material index out of range!");
            }
        }
        else
        {
            Debug.LogWarning("Screen renderer is NULL!");
        }

        // Update audio
        if (typingAudio != null)
        {
            if (isOn)
            {
                if (!typingAudio.isPlaying)
                {
                    typingAudio.Play();
                    Debug.Log("Typing audio started");
                }
            }
            else
            {
                if (typingAudio.isPlaying)
                {
                    typingAudio.Stop();
                    Debug.Log("Typing audio stopped");
                }
            }
        }
        else
        {
            Debug.LogWarning("Typing audio is NULL!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            InteractionHintUI.Instance?.ShowHint("Left click to use laptop");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            InteractionHintUI.Instance?.HideHint();

            if (isOn)
            {
                isOn = false;
                UpdateScreenAndAudio();
            }
        }
    }
}