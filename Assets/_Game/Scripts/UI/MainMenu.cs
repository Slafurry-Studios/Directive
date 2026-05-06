using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // ============ VARIABLES ============
    [Header("Scene Configuration")]
    [SerializeField]
    [Tooltip("The exact name of the main game scene to load when starting or continuing.")]
    private string gameSceneName = "GameScene";

    [SerializeField]
    [Tooltip("The exact name of the settings menu scene to load.")]
    private string settingsMenuSceneName = "SettingsMenu";

    [SerializeField]
    [Tooltip("The exact name of the about menu scene to load.")]
    private string aboutMenuSceneName = "AboutMenu";

    // ============ LOGIC ============
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Settings()
    {
        SceneManager.LoadScene(settingsMenuSceneName);
    }

    public void About()
    {
        SceneManager.LoadScene(aboutMenuSceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}