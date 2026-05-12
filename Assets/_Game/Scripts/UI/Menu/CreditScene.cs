using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditScene : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("Credit")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float scrollTime = 10f;

    [Header("Fade Image")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Scene")]
    [SerializeField] private string nextSceneName;

    private bool isAutoScrolling = false;

    private float visibleTime = 0f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        StartCoroutine(StartSequence());
    }

    void Update()
    {
        AutoScroll();
    }

    IEnumerator StartSequence()
    {
        // Fade In (hitam -> transparan)
        yield return StartCoroutine(FadeImage(1f, 0f));

        // Mulai scroll credit
        isAutoScrolling = true;
    }

    void AutoScroll()
    {
        if (!isAutoScrolling)
            return;

        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        visibleTime += Time.deltaTime;

        if (visibleTime >= scrollTime)
        {
            isAutoScrolling = false;

            // Langsung pindah scene
            SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator FadeImage(float startAlpha, float endAlpha)
    {
        float timer = 0f;

        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }
}