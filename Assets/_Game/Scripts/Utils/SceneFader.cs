using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fadeImage;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string targetScene;

    void OnEnable()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        if (fadeImage == null)
        {
            Debug.LogWarning("[SceneFader] fadeImage null — cek Inspector!");
            yield break;
        }

        fadeImage.gameObject.SetActive(true);
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        if (string.IsNullOrEmpty(targetScene))
        {
            Debug.LogWarning("[SceneFader] targetScene kosong — cek Inspector!");
            yield break;
        }

        SceneManager.LoadScene(targetScene);
    }
}