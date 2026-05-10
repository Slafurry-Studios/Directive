using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // ============ VARIABLES ============
    [Header("Scene Configuration")]
    [SerializeField] private string introSceneName = "Intro";
    [SerializeField] private string gameSceneName = "Game";

    [SerializeField]
    [Tooltip("The exact name of the settings menu scene to load.")]
    private string settingsMenuSceneName = "SettingsMenu";

    [SerializeField]
    [Tooltip("The exact name of the about menu scene to load.")]
    private string aboutMenuSceneName = "AboutMenu";

    [Header("ContinueBtn")]
    [SerializeField] private GameObject ContinueBtn;

    // ============ LOGIC ============

    void Start()
    {
        int progress = PlayerPrefs.GetInt("PlayerCheckpoint", 0);
        if (progress < 1)
        {
            ContinueBtn.SetActive(false);
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("PlayerCheckpoint", 0);
        SceneManager.LoadScene(introSceneName);
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