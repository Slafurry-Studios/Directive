using UnityEngine;
using System.Collections;

public class InvincibilityHandler : MonoBehaviour
{
    [SerializeField] private LayerMask invincibilityLayer;
    [SerializeField] private float invincibilityDuration = 1.0f;
    [SerializeField] private float blinkInterval = 0.1f;
   
    private Player player;
    private int originalLayer;

    void Start()
    {
        player = GetComponentInParent<Player>();
        originalLayer = gameObject.layer;
    }

    public void StartInvincibility()
    {
        gameObject.layer = MaskToLayer(invincibilityLayer);

        CancelInvoke(nameof(RestoreLayer));
        StopAllCoroutines();

        Invoke(nameof(RestoreLayer), invincibilityDuration);

        StartCoroutine(BlinkEffect());
    }

    private IEnumerator BlinkEffect()
    {
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            SetOpacity(0.3f);
            yield return new WaitForSeconds(blinkInterval);

            SetOpacity(1f);
            yield return new WaitForSeconds(blinkInterval);

            elapsed += blinkInterval * 2f;
        }

        SetOpacity(1f);
    }

    private void SetOpacity(float alpha)
    {

        foreach (SpriteRenderer sr in player.spriteRenderers)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }


    private void RestoreLayer()
    {
        gameObject.layer = originalLayer;
    }

    private int MaskToLayer(LayerMask mask)
    {
        int bitmask = mask.value;
        if (bitmask == 0) return 0;
        int result = 0;
        while (bitmask > 1)
        {
            bitmask >>= 1;
            result++;
        }
        return result;
    }
}