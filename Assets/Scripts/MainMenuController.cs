using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles button clicks on the Main Menu scene.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Tooltip("Exact name of the gameplay scene to load, must match the Scenes folder and Build Settings.")]
    public string gameplaySceneName = "GameScene";

    public void PlayGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
