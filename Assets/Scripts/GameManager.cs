using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central singleton that owns all game state: the water meter, the countdown timer,
/// and win/lose transitions. Every other script reads from or reports to this manager
/// instead of touching UI or game state directly, so everything stays in sync.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Meter Settings")]
    [Tooltip("Value the meter needs to reach to win (0-100).")]
    public float winThreshold = 100f;
    [Tooltip("How much the meter changes per clean water drop caught.")]
    public float waterGain = 8f;
    [Tooltip("How much the meter changes (drops) per pollution drop caught.")]
    public float pollutionLoss = 12f;

    [Header("Timer Settings")]
    [Tooltip("Total time, in seconds, the player has to fill the meter.")]
    public float startingTime = 60f;

    public float CurrentMeter { get; private set; }
    public float CurrentTime { get; private set; }
    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        // Simple singleton pattern: only one GameManager should exist at a time.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        CurrentMeter = 0f;
        CurrentTime = startingTime;
        IsGameOver = false;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMeter(CurrentMeter, winThreshold);
            UIManager.Instance.UpdateTimer(CurrentTime);
        }
    }

    private void Update()
    {
        if (IsGameOver) return;

        CurrentTime -= Time.deltaTime;
        if (CurrentTime <= 0f)
        {
            CurrentTime = 0f;
            LoseGame();
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimer(CurrentTime);
        }
    }

    /// <summary>
    /// Called by FallingItem when the player's bucket catches a drop.
    /// </summary>
    public void RegisterCatch(bool isClean)
    {
        if (IsGameOver) return;

        CurrentMeter += isClean ? waterGain : -pollutionLoss;
        CurrentMeter = Mathf.Clamp(CurrentMeter, 0f, winThreshold);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMeter(CurrentMeter, winThreshold);
        }

        if (AudioManager.Instance != null)
        {
            if (isClean) AudioManager.Instance.PlayCatchClean();
            else AudioManager.Instance.PlayCatchPollution();
        }

        if (CurrentMeter >= winThreshold)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayWin();
        if (UIManager.Instance != null) UIManager.Instance.ShowWinScreen();
    }

    private void LoseGame()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayLose();
        if (UIManager.Instance != null) UIManager.Instance.ShowLoseScreen();
    }

    /// <summary>
    /// Restarts the current gameplay scene. Hooked up to the "Play Again" buttons.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Returns to the main menu scene. Hooked up to the "Main Menu" buttons.
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
