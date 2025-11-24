using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    [Header("Target renderers to highlight")]
    public Renderer[] renderers;

    [Header("Highlight settings")]
    public Color highlightColor = new Color(0.6f, 1f, 0.6f, 1f);
    public float highlightIntensity = 3f;

    private Color[] originalEmissionColors;
    private bool[] originalEmissionEnabled;
    private bool initialized = false;

    void Awake()
    {
        if (renderers == null || renderers.Length == 0)
        {
            // Try to find a renderer on this object
            var r = GetComponent<Renderer>();
            if (r != null)
                renderers = new Renderer[] { r };
        }

        if (renderers != null && renderers.Length > 0)
        {
            originalEmissionColors = new Color[renderers.Length];
            originalEmissionEnabled = new bool[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                var mat = renderers[i].material;
                originalEmissionEnabled[i] = mat.IsKeywordEnabled("_EMISSION");
                originalEmissionColors[i] = mat.GetColor("_EmissionColor");
            }

            initialized = true;
        }
    }

    public void SetHighlighted(bool highlighted)
    {
        if (!initialized || renderers == null) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            var mat = renderers[i].material;

            if (highlighted)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", highlightColor * highlightIntensity);
            }
            else
            {
                if (originalEmissionEnabled[i])
                    mat.EnableKeyword("_EMISSION");
                else
                    mat.DisableKeyword("_EMISSION");

                mat.SetColor("_EmissionColor", originalEmissionColors[i]);
            }
        }
    }
}
