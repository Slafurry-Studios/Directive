using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TextScript : MonoBehaviour
{
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

    private readonly List<string> activeLines = new();

    private void Start()
    {
        Init();
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

    IEnumerator PlayDialog()
    {
        // =========================
        // PREPROCESSING
        // =========================

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

                processedLines.Add(
                    (
                        line.Trim(),
                        entry.color
                    )
                );
            }
        }

        // =========================
        // PLAY DIALOG
        // =========================

        foreach (var data in processedLines)
        {
            string coloredLine =
                $"<color={GetColor(data.color)}>{data.text}</color>";

            // tambah line baru
            activeLines.Add(coloredLine);

            // tampilkan semua line lama dulu
            textUI.text = BuildTextWithoutLast();

            // typing line terakhir saja
            yield return StartCoroutine(
                TypeLastLine(coloredLine)
            );

            yield return null;

            textUI.ForceMeshUpdate();

            // hapus line atas jika visual line melebihi batas
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
    }

    string BuildFullText()
    {
        return string.Join("\n", activeLines);
    }

    string BuildTextWithoutLast()
    {
        if (activeLines.Count <= 1)
            return "";

        return string.Join(
            "\n",
            activeLines.GetRange(0, activeLines.Count - 1)
        );
    }

    IEnumerator TypeLastLine(string richText)
    {
        // tambah enter jika sudah ada text sebelumnya
        if (!string.IsNullOrEmpty(textUI.text))
            textUI.text += "\n";

        bool insideTag = false;

        foreach (char c in richText)
        {
            if (c == '<')
                insideTag = true;

            textUI.text += c;

            // delay hanya untuk karakter visible
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

            // neon green
            TextColor.Green => "#48FF00",

            // bright red
            TextColor.Red => "#FF3030",

            _ => "white"
        };
    }
}