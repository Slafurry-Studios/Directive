using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TextScript : MonoBehaviour
{
    [Header("UI (Index-based)")]
    [Tooltip("Urutan index menentukan urutan dialog")]
    public List<TextMeshProUGUI> textUIList;

    [Header("Data")]
    public DialogData dialogData;

    [Header("Controller")]
    public TextBox textBox;

    [Header("Dialog Settings")]
    [SerializeField] private float delayBetweenDialogs = 0.5f;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        // fallback
        if (textBox == null)
            textBox = FindAnyObjectByType<TextBox>();

        if (textBox == null)
        {
            Debug.LogError("TextBox tidak ditemukan!");
            return;
        }

        if (dialogData == null)
        {
            Debug.LogError("DialogData belum di-assign!");
            return;
        }

        if (textUIList == null || textUIList.Count == 0)
        {
            Debug.LogError("Text UI List kosong!");
            return;
        }

        // jalankan sesuai index
        StartCoroutine(PlayByIndex());
    }

    System.Collections.IEnumerator PlayByIndex()
    {
        int count = Mathf.Min(textUIList.Count, dialogData.dialogEntries.Count);

        for (int i = 0; i < count; i++)
        {
            var ui = textUIList[i];
            var entry = dialogData.dialogEntries[i];

            if (ui == null) continue;

            // Bersihkan text sebelum typing dimulai
            ui.text = "";

            // jalankan typing dengan UI yang sesuai (cursor akan berkedip selama proses)
            yield return StartCoroutine(
                textBox.TypeText(ui, entry.text, entry.color)
            );

            // Cursor tetap berkedip, tunggu sebelum dialog berikutnya
            yield return new WaitForSeconds(delayBetweenDialogs);

            // Stop cursor sebelum dialog berikutnya
            textBox.StopCursor(ui);
        }
    }
}