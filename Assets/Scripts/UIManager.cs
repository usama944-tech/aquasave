using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Owns all in-game UI: the water meter fill bar, the countdown text, and the
/// win/lose panels. GameManager calls into this rather than several scripts
/// updating UI independently, so the UI can never fall out of sync with state.
/// Uses Unity's built-in UI.Text (rather than TextMeshPro) so the whole scene
/// works immediately with no "Import TMP Essentials" step required.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Meter UI")]
    public Image meterFillImage; // Image.type = Filled, horizontal
    public Text meterLabel;

    [Header("Timer UI")]
    public Text timerLabel;

    [Header("End Screens")]
    public GameObject winPanel;
    public GameObject losePanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    public void UpdateMeter(float current, float max)
    {
        float pct = max > 0f ? current / max : 0f;

        if (meterFillImage != null)
        {
            meterFillImage.fillAmount = pct;
        }

        if (meterLabel != null)
        {
            meterLabel.text = $"{Mathf.RoundToInt(pct * 100f)}%";
        }
    }

    public void UpdateTimer(float secondsRemaining)
    {
        if (timerLabel == null) return;

        int seconds = Mathf.CeilToInt(Mathf.Max(secondsRemaining, 0f));
        int minutes = seconds / 60;
        int remSeconds = seconds % 60;
        timerLabel.text = $"{minutes:00}:{remSeconds:00}";
    }

    public void ShowWinScreen()
    {
        if (winPanel != null) winPanel.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        if (losePanel != null) losePanel.SetActive(true);
    }
}
