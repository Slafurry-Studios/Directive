using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [SerializeField] private LayerMask invincibilityLayer;
    [SerializeField] private float invincibilityDuration = 1.0f;
    [SerializeField] private float blinkInterval = 0.1f;

    private int originalLayer;
    private Animator animator;
    private SpriteRenderer[] spriteRenderers;

    protected override void Start()
    {
        base.Start();

        OnHealthChanged += HandlePlayerHit;
        OnDeath += HandlePlayerDeath;

        animator = GetComponent<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        originalLayer = gameObject.layer;

        UpdateUI(currentHealth, maxHealth);
    }

    private void HandlePlayerHit(int current, int max)
    {
        animator.SetTrigger("onHit");

        gameObject.layer = MaskToLayer(invincibilityLayer);

        CancelInvoke(nameof(RestoreLayer));
        StopAllCoroutines();

        Invoke(nameof(RestoreLayer), invincibilityDuration);

        StartCoroutine(BlinkEffect());

        UpdateUI(current, max);
    }

    private System.Collections.IEnumerator BlinkEffect()
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
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }

    private void UpdateUI(int current, int max)
    {
        if (HealthHUD.Instance != null)
        {
            HealthHUD.Instance.UpdateUI(current, max);
        }
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player died! Show Game Over.");
        RestartLevel();
    }

    public override void ApplyKnockback(Vector2 direction, float force)
    {
        PlayerMove move = GetComponent<PlayerMove>();

        if (move != null)
        {
            move.enabled = false;
        }

        base.ApplyKnockback(direction, force);

        Invoke(nameof(EnableMovement), 0.5f);
    }

    private void EnableMovement()
    {
        PlayerMove move = GetComponent<PlayerMove>();

        if (move != null)
        {
            move.enabled = true;
        }
    }

    private void RestartLevel()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    private void RestoreLayer()
    {
        gameObject.layer = originalLayer;
    }

    private int MaskToLayer(LayerMask mask)
    {
        int bitmask = mask.value;

        if (bitmask == 0)
        {
            return 0;
        }

        int result = 0;

        while (bitmask > 1)
        {
            bitmask >>= 1;
            result++;
        }

        return result;
    }
}