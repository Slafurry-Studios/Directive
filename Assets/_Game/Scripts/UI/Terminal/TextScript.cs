using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TextScript : MonoBehaviour
{
    public enum DisplayMode
    {
        Typewriter,
        PerLine
    }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textUI;

    [Header("Data")]
    [SerializeField] private DialogData dialogData;

    [Header("Controller")]
    [SerializeField] private TextBox textBox;

    [Header("Settings")]
    [SerializeField] private int maxLines = 4;
    [SerializeField] private float delayBetweenDialogs = 0.5f;
    [SerializeField] private float typingSpeed = 0.03f;
    [SerializeField] private DisplayMode displayMode = DisplayMode.Typewriter;

    [Header("Typing Audio")]
    [SerializeField] private AudioSource typeAudioSource;
    [SerializeField] private AudioClip typeAudioClip;

    [Header("Fade Out Audio Sources")]
    [SerializeField] private List<AudioSource> fadeOutAudioSources = new();
    [SerializeField] private float fadeOutDuration = 1f;

    [Header("Scene Transition")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private float delayBeforeSceneChange = 1f;

    [Header("Activate On Finish")]
    [SerializeField] private List<GameObject> objectsToActivate = new();

    private readonly List<string> activeLines = new();

    private void Start()
    {
        Init();
    }
    void ActivateObjects()
    {
        if (objectsToActivate == null || objectsToActivate.Count == 0)
            return;

        foreach (GameObject obj in objectsToActivate)
        {
            if (obj == null)
                continue;

            obj.SetActive(true);
        }
    }

    void Init()
    {
        if (textBox == null)
            textBox = FindAnyObjectByType<TextBox>();

        if (textUI == null || dialogData == null || textBox == null)
        {
            Debug.LogError("Reference ada yang kosong!");
            return;
        }

        textUI.text = "";

        StartCoroutine(PlayDialog());
    }

    void StartLoopAudio()
    {
        if (typeAudioSource == null || typeAudioClip == null)
            return;

        typeAudioSource.clip = typeAudioClip;
        typeAudioSource.loop = true;
        typeAudioSource.Play();
    }

    void StopLoopAudio()
    {
        if (typeAudioSource == null)
            return;

        typeAudioSource.Stop();
    }

    IEnumerator PlayDialog()
    {
        List<(string text, TextColor color)> processedLines = new();

        foreach (var entry in dialogData.dialogEntries)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.text))
                continue;

            string[] splitLines = entry.text.Split('\n');

            foreach (string line in splitLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                processedLines.Add((line.Trim(), entry.color));
            }
        }

        foreach (var data in processedLines)
        {
            string coloredLine = $"<color={GetColor(data.color)}>{data.text}</color>";

            activeLines.Add(coloredLine);

            textUI.text = BuildTextWithoutLast();

            if (displayMode == DisplayMode.Typewriter)
            {
                StartLoopAudio();

                yield return StartCoroutine(TypeLastLine(coloredLine));

                StopLoopAudio();
            }
            else
            {
                if (!string.IsNullOrEmpty(textUI.text))
                    textUI.text += "\n";

                textUI.text += coloredLine;
            }

            yield return null;

            textUI.ForceMeshUpdate();

            while (textUI.textInfo.lineCount > maxLines)
            {
                if (activeLines.Count <= 1)
                    break;

                activeLines.RemoveAt(0);

                textUI.text = BuildFullText();

                textUI.ForceMeshUpdate();
            }

            yield return new WaitForSeconds(delayBetweenDialogs);

            textBox.StopCursor(textUI);
        }

        yield return StartCoroutine(FadeOutAllAudio());
        ActivateObjects();

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            yield return new WaitForSeconds(delayBeforeSceneChange);
            SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator FadeOutAllAudio()
    {
        if (fadeOutAudioSources == null || fadeOutAudioSources.Count == 0)
            yield break;


        MusicPlayer.Instance.StopMusic(0.4f);

        Dictionary<AudioSource, float> startVolumes = new();

        foreach (AudioSource source in fadeOutAudioSources)
        {
            if (source == null)
                continue;

            startVolumes[source] = source.volume;
        }

        float time = 0f;

        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;

            float t = time / fadeOutDuration;

            foreach (AudioSource source in fadeOutAudioSources)
            {
                if (source == null)
                    continue;

                source.volume = Mathf.Lerp(startVolumes[source], 0f, t);
            }

            yield return null;
        }

        foreach (AudioSource source in fadeOutAudioSources)
        {
            if (source == null)
                continue;

            source.volume = 0f;
            source.Stop();
        }
    }

    string BuildFullText()
    {
        return string.Join("\n", activeLines);
    }

    string BuildTextWithoutLast()
    {
        if (activeLines.Count <= 1)
            return "";

        return string.Join("\n", activeLines.GetRange(0, activeLines.Count - 1));
    }

    IEnumerator TypeLastLine(string richText)
    {
        if (!string.IsNullOrEmpty(textUI.text))
            textUI.text += "\n";

        bool insideTag = false;

        foreach (char c in richText)
        {
            if (c == '<')
                insideTag = true;

            textUI.text += c;

            if (!insideTag)
                yield return new WaitForSeconds(typingSpeed);

            if (c == '>')
                insideTag = false;
        }
    }

    string GetColor(TextColor color)
    {
        return color switch
        {
            TextColor.White => "white",
            TextColor.Purple => "#B86CFF",
            TextColor.Green => "#48FF00",
            TextColor.Red => "#FF3030",
            _ => "white"
        };
    }
}