using UnityEngine;
using System.Collections;

public class HitPause : MonoBehaviour
{
    public static HitPause Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Pause(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(DoPause(duration));
    }

    private IEnumerator DoPause(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}