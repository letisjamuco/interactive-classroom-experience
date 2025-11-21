using UnityEngine;

public class DayNightSwitch : MonoBehaviour
{
    public Light sun;
    public Material daySkybox;
    public Material nightSkybox;

    public bool isDay = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) // Press N to toggle day/night
        {
            ToggleDayNight();
        }
    }

    void ToggleDayNight()
    {
        isDay = !isDay;

        if (isDay)
        {
            // DAY
            RenderSettings.skybox = daySkybox;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            sun.intensity = 1.2f;
            RenderSettings.reflectionIntensity = 1f;
        }
        else
        {
            // NIGHT
            RenderSettings.skybox = nightSkybox;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = Color.black;
            sun.intensity = 0f;
            RenderSettings.reflectionIntensity = 0f;
        }
    }
}
