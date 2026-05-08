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

    private readonly Queue<string> lines = new();

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
        foreach (var entry in dialogData.dialogEntries)
        {
            // convert jadi rich text color
            string coloredLine =
                $"<color={GetColor(entry.color)}>{entry.text}</color>";

            // tambah line baru
            lines.Enqueue(coloredLine);

            // hapus line atas jika melebihi limit
            while (lines.Count > maxLines)
            {
                lines.Dequeue();
            }

            // ambil semua line
            string[] allLines = lines.ToArray();

            // ambil semua kecuali line terakhir
            string previousText = "";

            for (int i = 0; i < allLines.Length - 1; i++)
            {
                previousText += allLines[i] + "\n";
            }

            // tampilkan text lama dulu
            textUI.text = previousText;

            // typing line terakhir saja
            yield return StartCoroutine(
                TypeLastLine(coloredLine)
            );

            yield return new WaitForSeconds(delayBetweenDialogs);

            textBox.StopCursor(textUI);
        }
    }

    IEnumerator TypeLastLine(string richText)
    {
        bool insideTag = false;

        foreach (char c in richText)
        {
            if (c == '<')
                insideTag = true;

            textUI.text += c;

            if (!insideTag)
            {
                yield return new WaitForSeconds(typingSpeed);
            }

            if (c == '>')
                insideTag = false;
        }
    }

    string GetColor(TextColor color)
    {
        return color switch
        {
            TextColor.White => "white",

            // ungu tetap
            TextColor.Purple => "#B86CFF",

            // hijau lebih terang neon
            TextColor.Green => "#48ff00",

            // merah lebih terang vivid
            TextColor.Red => "#690404",

            _ => "white"
        };
    }
}