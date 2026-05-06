using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextPopupManager : MonoBehaviour
{
    public static TextPopupManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TextFade[] textFades;

    [Header("Settings")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float maxInterval = 3f;

    [Header("Speedrun Settings")]
    [SerializeField] private bool instantClearOnNewSequence = true;
    [SerializeField] private float quickFadeOutDuration = 0.2f;
    private List<int> availableIndices;
    private Coroutine randomSequence;
    private Coroutine sequentialSequence;
    private HashSet<int> activeTextIndices = new HashSet<int>();
    private List<Coroutine> activeCoroutines = new List<Coroutine>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        foreach (var textFade in textFades)
        {
            if (textFade != null)
            {
                textFade.Hide();
            }
        }

        ResetPool();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    #region Sequential Methods

    public void ShowMessagesSequential(List<string> messages, float interval = -1f)
    {
        ShowMessagesSequential(messages, Color.white, interval);
    }

    public void ShowMessagesSequential(List<string> messages, Color color, float interval = -1f)
    {
        if (messages == null || messages.Count == 0)
        {
            Debug.LogWarning("TextPopupManager: Empty message list");
            return;
        }

        ClearCurrentSequence();

        float useInterval = interval >= 0 ? interval : minInterval;
        sequentialSequence = StartCoroutine(SequentialFadeCoroutine(messages, color, useInterval));
    }

    public void ShowMessagesSequential(List<string> messages, float interval, float fadeIn, float display, float fadeOut)
    {
        ShowMessagesSequential(messages, Color.white, interval, fadeIn, display, fadeOut);
    }

    public void ShowMessagesSequential(List<string> messages, Color color, float interval, float fadeIn, float display, float fadeOut)
    {
        if (messages == null || messages.Count == 0)
        {
            Debug.LogWarning("TextPopupManager: Empty message list");
            return;
        }

        ClearCurrentSequence();

        sequentialSequence = StartCoroutine(SequentialFadeCoroutine(messages, color, interval, fadeIn, display, fadeOut));
    }

    public void ShowMessagesSequential(string[] messages, float interval = -1f)
    {
        ShowMessagesSequential(messages, Color.white, interval);
    }

    public void ShowMessagesSequential(string[] messages, Color color, float interval = -1f)
    {
        if (messages == null || messages.Length == 0)
        {
            Debug.LogWarning("TextPopupManager: Empty message array");
            return;
        }

        ShowMessagesSequential(new List<string>(messages), color, interval);
    }

    #endregion

    #region Random Multiple Methods

    public void ShowMessagesRandom(List<string> messages)
    {
        ShowMessagesRandom(messages, Color.white);
    }

    public void ShowMessagesRandom(List<string> messages, Color color)
    {
        if (messages == null || messages.Count == 0)
        {
            Debug.LogWarning("TextPopupManager: Empty message list");
            return;
        }

        ClearCurrentSequence();

        ResetPool();

        foreach (string message in messages)
        {
            int textIndex = GetRandomIndex();

            if (textIndex >= 0 && textIndex < textFades.Length)
            {
                activeTextIndices.Add(textIndex);
                textFades[textIndex].FadeInOut(message, color, fadeInDuration, displayDuration, fadeOutDuration);
            }
            else
            {
                Debug.LogWarning($"TextPopupManager: Not enough text slots for message: {message}");
                break;
            }
        }
    }

    #endregion

    #region Single Message Methods

    public void ShowMessage(int textIndex, string message)
    {
        ShowMessage(textIndex, message, Color.white);
    }

    public void ShowMessage(int textIndex, string message, Color color)
    {
        if (textIndex >= 0 && textIndex < textFades.Length && textFades[textIndex] != null)
        {
            if (activeTextIndices.Contains(textIndex))
            {
                textFades[textIndex].StopFade();
            }

            activeTextIndices.Add(textIndex);
            textFades[textIndex].FadeInOut(message, color, fadeInDuration, displayDuration, fadeOutDuration);

            StartCoroutine(RemoveFromActiveAfterDelay(textIndex, fadeInDuration + displayDuration + fadeOutDuration));
        }
        else
        {
            Debug.LogWarning($"TextPopupManager: Invalid text index {textIndex}");
        }
    }

    public void ShowMessage(int textIndex, string message, float fadeIn, float display, float fadeOut)
    {
        ShowMessage(textIndex, message, Color.white, fadeIn, display, fadeOut);
    }

    public void ShowMessage(int textIndex, string message, Color color, float fadeIn, float display, float fadeOut)
    {
        if (textIndex >= 0 && textIndex < textFades.Length && textFades[textIndex] != null)
        {
            if (activeTextIndices.Contains(textIndex))
            {
                textFades[textIndex].StopFade();
            }

            activeTextIndices.Add(textIndex);
            textFades[textIndex].FadeInOut(message, color, fadeIn, display, fadeOut);

            StartCoroutine(RemoveFromActiveAfterDelay(textIndex, fadeIn + display + fadeOut));
        }
        else
        {
            Debug.LogWarning($"TextPopupManager: Invalid text index {textIndex}");
        }
    }

    public void ShowMessageRandom(string message)
    {
        ShowMessageRandom(message, Color.white);
    }

    public void ShowMessageRandom(string message, Color color)
    {
        int textIndex = GetRandomIndex();

        if (textIndex >= 0 && textIndex < textFades.Length)
        {
            activeTextIndices.Add(textIndex);
            textFades[textIndex].FadeInOut(message, color, fadeInDuration, displayDuration, fadeOutDuration);
            StartCoroutine(RemoveFromActiveAfterDelay(textIndex, fadeInDuration + displayDuration + fadeOutDuration));
        }
        else
        {
            ResetPool();
            textIndex = GetRandomIndex();

            if (textIndex >= 0 && textIndex < textFades.Length)
            {
                activeTextIndices.Add(textIndex);
                textFades[textIndex].FadeInOut(message, color, fadeInDuration, displayDuration, fadeOutDuration);
                StartCoroutine(RemoveFromActiveAfterDelay(textIndex, fadeInDuration + displayDuration + fadeOutDuration));
            }
        }
    }

    public void ShowMessageRandom(string message, float fadeIn, float display, float fadeOut)
    {
        ShowMessageRandom(message, Color.white, fadeIn, display, fadeOut);
    }

    public void ShowMessageRandom(string message, Color color, float fadeIn, float display, float fadeOut)
    {
        int textIndex = GetRandomIndex();

        if (textIndex >= 0 && textIndex < textFades.Length)
        {
            activeTextIndices.Add(textIndex);
            textFades[textIndex].FadeInOut(message, color, fadeIn, display, fadeOut);
            StartCoroutine(RemoveFromActiveAfterDelay(textIndex, fadeIn + display + fadeOut));
        }
        else
        {
            ResetPool();
            textIndex = GetRandomIndex();

            if (textIndex >= 0 && textIndex < textFades.Length)
            {
                activeTextIndices.Add(textIndex);
                textFades[textIndex].FadeInOut(message, color, fadeIn, display, fadeOut);
                StartCoroutine(RemoveFromActiveAfterDelay(textIndex, fadeIn + display + fadeOut));
            }
        }
    }

    #endregion

    #region Control Methods

    public void StopSequential()
    {
        if (sequentialSequence != null)
        {
            StopCoroutine(sequentialSequence);
            sequentialSequence = null;
        }
    }

    public void StopAll()
    {
        StopRandomFade();
        StopSequential();
        StopAllActiveCoroutines();
        activeTextIndices.Clear();
    }

    public void HideAll()
    {
        StopAll();
        foreach (var textFade in textFades)
        {
            if (textFade != null)
            {
                textFade.Hide();
            }
        }
    }

    public void ShowAll()
    {
        StopAll();
        foreach (var textFade in textFades)
        {
            if (textFade != null)
            {
                textFade.Show();
            }
        }
    }

    public void StopTextFade(int textIndex)
    {
        if (textIndex >= 0 && textIndex < textFades.Length && textFades[textIndex] != null)
        {
            textFades[textIndex].StopFade();
            activeTextIndices.Remove(textIndex);
        }
    }

    public int GetTextCount()
    {
        return textFades != null ? textFades.Length : 0;
    }

    /// <summary>
    /// Clear sequence yang sedang berjalan dengan cepat
    /// </summary>
    private void ClearCurrentSequence()
    {
        // Stop semua coroutine sequence
        StopSequential();
        StopRandomFade();
        StopAllActiveCoroutines();

        if (instantClearOnNewSequence)
        {
            // Langsung hide semua text yang aktif
            foreach (int index in activeTextIndices)
            {
                if (index >= 0 && index < textFades.Length && textFades[index] != null)
                {
                    textFades[index].Hide();
                }
            }
            activeTextIndices.Clear();
        }
        else
        {
            // Fade out cepat text yang aktif
            foreach (int index in activeTextIndices)
            {
                if (index >= 0 && index < textFades.Length && textFades[index] != null)
                {
                    StartCoroutine(QuickFadeOut(index));
                }
            }
            activeTextIndices.Clear();
        }

        ResetPool();
    }

    private void StopAllActiveCoroutines()
    {
        foreach (var coroutine in activeCoroutines)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        activeCoroutines.Clear();
    }

    #endregion

    #region Random Fade Auto

    public void StartRandomFade()
    {
        StopRandomFade();
        randomSequence = StartCoroutine(RandomFadeSequence());
    }

    public void StopRandomFade()
    {
        if (randomSequence != null)
        {
            StopCoroutine(randomSequence);
            randomSequence = null;
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator SequentialFadeCoroutine(List<string> messages, Color color, float interval)
    {
        return SequentialFadeCoroutine(messages, color, interval, fadeInDuration, displayDuration, fadeOutDuration);
    }

    private IEnumerator SequentialFadeCoroutine(List<string> messages, Color color, float interval, float fadeIn, float display, float fadeOut)
    {
        ResetPool();

        foreach (string message in messages)
        {
            int textIndex = GetRandomIndex();

            if (textIndex >= 0 && textIndex < textFades.Length)
            {
                activeTextIndices.Add(textIndex);
                textFades[textIndex].FadeInOut(message, color, fadeIn, display, fadeOut);

                Coroutine removeCoroutine = StartCoroutine(RemoveFromActiveAfterDelay(textIndex, fadeIn + display + fadeOut));
                activeCoroutines.Add(removeCoroutine);

                yield return new WaitForSeconds(interval);
            }
            else
            {
                ResetPool();
                textIndex = GetRandomIndex();

                if (textIndex >= 0 && textIndex < textFades.Length)
                {
                    activeTextIndices.Add(textIndex);
                    textFades[textIndex].FadeInOut(message, color, fadeIn, display, fadeOut);

                    Coroutine removeCoroutine = StartCoroutine(RemoveFromActiveAfterDelay(textIndex, fadeIn + display + fadeOut));
                    activeCoroutines.Add(removeCoroutine);

                    yield return new WaitForSeconds(interval);
                }
            }
        }

        sequentialSequence = null;
    }

    private IEnumerator RandomFadeSequence()
    {
        while (true)
        {
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            int textIndex = GetRandomIndex();

            if (textIndex >= 0 && textIndex < textFades.Length)
            {
                activeTextIndices.Add(textIndex);
                textFades[textIndex].FadeInOut(fadeInDuration, displayDuration, fadeOutDuration);

                Coroutine removeCoroutine = StartCoroutine(RemoveFromActiveAfterDelay(textIndex, fadeInDuration + displayDuration + fadeOutDuration));
                activeCoroutines.Add(removeCoroutine);
            }
            else
            {
                ResetPool();
            }
        }
    }

    private IEnumerator RemoveFromActiveAfterDelay(int textIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        activeTextIndices.Remove(textIndex);
    }

    private IEnumerator QuickFadeOut(int textIndex)
    {
        if (textIndex >= 0 && textIndex < textFades.Length && textFades[textIndex] != null)
        {
            textFades[textIndex].StopFade();

            // Implementasi quick fade out - sesuaikan dengan TextFade class Anda
            CanvasGroup canvasGroup = textFades[textIndex].GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                float elapsed = 0f;
                float startAlpha = canvasGroup.alpha;

                while (elapsed < quickFadeOutDuration)
                {
                    elapsed += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / quickFadeOutDuration);
                    yield return null;
                }

                canvasGroup.alpha = 0f;
            }

            textFades[textIndex].Hide();
        }
    }

    #endregion

    #region Helper Methods

    private int GetRandomIndex()
    {
        if (availableIndices == null || availableIndices.Count == 0)
            return -1;

        int randomIdx = Random.Range(0, availableIndices.Count);
        int textIndex = availableIndices[randomIdx];
        availableIndices.RemoveAt(randomIdx);

        return textIndex;
    }

    private void ResetPool()
    {
        availableIndices = new List<int>();
        if (textFades != null)
        {
            for (int i = 0; i < textFades.Length; i++)
            {
                if (textFades[i] != null)
                {
                    availableIndices.Add(i);
                }
            }
        }
    }

    #endregion
}