using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class OverlayEffect : MonoBehaviour
{
    [Header("Damage Overlay")]
    private Image damageOverlay;
    [SerializeField] private float overlayPeakAlpha = 0.4f;
    [SerializeField] private float overlayFadeDuration = 0.4f;

    [Header("Camera Shake")]
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private float shakeForce = 1f;

    void Start()
    {
        GameObject overlay = GameObject.FindGameObjectWithTag("Overlay");
        damageOverlay = overlay.GetComponent<Image>();

        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (damageOverlay != null)
            SetOverlayAlpha(0f);
    }

    public void StartDamageEffect()
    {
        StartCoroutine(DamageOverlayEffect());
        TriggerCameraShake();
    }

    private void TriggerCameraShake()
    {
        if (impulseSource != null)
            impulseSource.GenerateImpulse(shakeForce);
    }

    private IEnumerator DamageOverlayEffect()
    {
        if (damageOverlay == null) yield break;

        SetOverlayAlpha(overlayPeakAlpha);

        float elapsed = 0f;
        while (elapsed < overlayFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(overlayPeakAlpha, 0f, elapsed / overlayFadeDuration);
            SetOverlayAlpha(alpha);
            yield return null;
        }

        SetOverlayAlpha(0f);
    }

    private void SetOverlayAlpha(float alpha)
    {
        Color color = damageOverlay.color;
        color.a = alpha;
        damageOverlay.color = color;
    }
}