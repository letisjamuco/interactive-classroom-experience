using UnityEngine;
using TMPro;

public class InteractionHintUI : MonoBehaviour
{
    public static InteractionHintUI Instance;

    public TextMeshProUGUI hintText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }

    public void ShowHint(string message)
    {
        if (hintText == null) return;

        hintText.text = message;
        hintText.gameObject.SetActive(true);
    }

    public void HideHint()
    {
        if (hintText == null) return;
        hintText.gameObject.SetActive(false);
    }
}
