using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextFade : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void FadeInOut(float fadeInDuration, float displayDuration, float fadeOutDuration)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeSequence(fadeInDuration, displayDuration, fadeOutDuration));
    }

    public void FadeInOut(string text, float fadeInDuration, float displayDuration, float fadeOutDuration)
    {
        SetText(text);
        FadeInOut(fadeInDuration, displayDuration, fadeOutDuration);
    }

    public void FadeInOut(string text, Color color, float fadeInDuration, float displayDuration, float fadeOutDuration)
    {
        SetText(text);
        SetColor(color);
        FadeInOut(fadeInDuration, displayDuration, fadeOutDuration);
    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }

    public string GetText()
    {
        return textMesh.text;
    }
    public void SetColor(Color color)
    {
        Color newColor = color;
        newColor.a = textMesh.color.a; 
        textMesh.color = newColor;
    }

    public Color GetColor()
    {
        return textMesh.color;
    }

    public TextMeshProUGUI GetTextMesh()
    {
        return textMesh;
    }

    private IEnumerator FadeSequence(float fadeIn, float display, float fadeOut)
    {
        yield return Fade(0f, 1f, fadeIn);
        yield return new WaitForSeconds(display);
        yield return Fade(1f, 0f, fadeOut);

        fadeCoroutine = null;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(endAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = textMesh.color;
        color.a = alpha;
        textMesh.color = color;
    }

    public void StopFade()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }

    public void SetAlphaInstant(float alpha)
    {
        StopFade();
        SetAlpha(alpha);
    }

    public void Show()
    {
        SetAlphaInstant(1f);
    }

    public void Hide()
    {
        SetAlphaInstant(0f);
    }

    public bool IsFading()
    {
        return fadeCoroutine != null;
    }
}