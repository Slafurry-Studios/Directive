using UnityEngine;
using TMPro;
using System.Collections;

public class TextBox : MonoBehaviour
{
    [Header("Typing Settings")]
    [SerializeField] private int lettersPerSecond = 20;

    [Header("Cursor Settings")]
    [SerializeField] private string cursorSymbol = "I";
    [SerializeField] private float cursorBlinkSpeed = 0.5f;

    private Coroutine cursorCoroutine;

    // =========================
    // TYPE TEXT
    // =========================
    public IEnumerator TypeText(TextMeshProUGUI targetUI, string text, TextColor color)
    {
        // stop cursor lama
        StopCursor(targetUI);

        targetUI.text = "";
        targetUI.color = GetColor(color);

        // mulai blinking cursor SEBELUM typing (cursor aktif di setiap huruf)
        cursorCoroutine = StartCoroutine(BlinkCursor(targetUI));

        foreach (char letter in text)
        {
            targetUI.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        // cursor tetap berkedip setelah typing selesai (tidak dihentikan)
    }

    // =========================
    // CURSOR BLINK (FIXED)
    // =========================
    public IEnumerator BlinkCursor(TextMeshProUGUI targetUI)
    {
        bool visible = true;

        while (true)
        {
            // Hapus SEMUA cursor dulu (untuk prevent cursor di tengah text)
            string cleanText = targetUI.text.Replace(cursorSymbol, "");
            
            if (visible && !string.IsNullOrEmpty(cleanText))
            {
                // Tambah cursor hanya di akhir jika visible dan text tidak kosong
                targetUI.text = cleanText + cursorSymbol;
            }
            else
            {
                // Tampilkan text tanpa cursor
                targetUI.text = cleanText;
            }

            visible = !visible;
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }

    // =========================
    // STOP CURSOR (PENTING)
    // =========================
    public void StopCursor(TextMeshProUGUI targetUI)
    {
        if (cursorCoroutine != null)
        {
            StopCoroutine(cursorCoroutine);
            cursorCoroutine = null;
        }

        RemoveCursor(targetUI);
    }

    // =========================
    // REMOVE CURSOR
    // =========================
    public void RemoveCursor(TextMeshProUGUI targetUI)
    {
        if (targetUI != null && targetUI.text.EndsWith(cursorSymbol))
        {
            targetUI.text = targetUI.text.Substring(0, targetUI.text.Length - cursorSymbol.Length);
        }
    }

    // =========================
    // COLOR
    // =========================
    Color GetColor(TextColor color)
    {
        switch (color)
        {
            case TextColor.Purple: return new Color(0.7f, 0.3f, 1f);
            case TextColor.Green: return Color.green;
            case TextColor.Red: return Color.red;
            default: return Color.white;
        }
    }
}