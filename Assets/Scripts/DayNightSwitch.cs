using UnityEngine;
using UnityEngine.EventSystems;

public class DayNightSwitch : MonoBehaviour
{
    [Header("Skyboxes")]
    public Material daySkybox;
    public Material nightSkybox;

    [Header("Sun Light")]
    public Light sun;
    public float nightSunIntensity = 0f;

    [Header("Input")]
    public KeyCode toggleKey = KeyCode.N;

    [Header("References")]
    [Tooltip("Reference to player controller to check if notebook is open")]
    public SimplePlayerController playerController;

    private bool isNight = false;

    // Cached initial values
    private Material originalSkybox;
    private float originalAmbientIntensity;
    private float originalSunIntensity;
    private Color originalSunColor;

    [System.Obsolete]
    void Start()
    {
        // Cache initial environment so we can restore it exactly
        originalSkybox = RenderSettings.skybox;
        originalAmbientIntensity = RenderSettings.ambientIntensity;

        if (sun != null)
        {
            originalSunIntensity = sun.intensity;
            originalSunColor = sun.color;
        }

        // Use explicit day skybox if set
        if (daySkybox != null)
        {
            RenderSettings.skybox = daySkybox;
            DynamicGI.UpdateEnvironment();
        }

        // Try to find player controller if not set
        if (playerController == null)
        {
            playerController = FindObjectOfType<SimplePlayerController>();
        }
    }

    // Check if user is typing in a UI input field
    private bool IsTypingInUI()
    {
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem != null && eventSystem.currentSelectedGameObject != null)
        {
            UnityEngine.UI.InputField inputField = eventSystem.currentSelectedGameObject.GetComponent<UnityEngine.UI.InputField>();
            TMPro.TMP_InputField tmpInputField = eventSystem.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>();

            return (inputField != null || tmpInputField != null);
        }
        return false;
    }

    void Update()
    {
        // CRITICAL: Don't toggle if notebook is open OR if user is typing in UI
        if ((playerController != null && playerController.notebookOpen) || IsTypingInUI())
        {
            return;
        }

        if (Input.GetKeyDown(toggleKey))
        {
            if (!isNight)
                SetNight();
            else
                SetDay();
        }
    }

    void SetNight()
    {
        isNight = true;

        if (nightSkybox != null)
            RenderSettings.skybox = nightSkybox;

        RenderSettings.ambientIntensity = 0.2f;

        if (sun != null)
            sun.intensity = nightSunIntensity;

        DynamicGI.UpdateEnvironment();

        Debug.Log("Switched to NIGHT");
    }

    void SetDay()
    {
        isNight = false;

        if (daySkybox != null)
            RenderSettings.skybox = daySkybox;
        else if (originalSkybox != null)
            RenderSettings.skybox = originalSkybox;

        RenderSettings.ambientIntensity = originalAmbientIntensity;

        if (sun != null)
        {
            sun.intensity = originalSunIntensity;
            sun.color = originalSunColor;
        }

        DynamicGI.UpdateEnvironment();

        Debug.Log("Switched to DAY");
    }
}