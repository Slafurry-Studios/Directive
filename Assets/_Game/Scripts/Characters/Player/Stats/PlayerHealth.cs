using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [SerializeField] private LayerMask invincibilityLayer;
    [SerializeField] private float invincibilityDuration = 1.0f;
    
    private int originalLayer;
    private Animator animator;
    protected override void Start()
    {
        base.Start();
        OnHealthChanged += HandlePlayerHit;
        OnDeath += HandlePlayerDeath;
        animator = GetComponent<Animator>();

        originalLayer = gameObject.layer;

        UpdateUI(currentHealth, maxHealth);
    }

    private void HandlePlayerHit(int current, int max)
    {
        animator.SetTrigger("onHit");

        gameObject.layer = MaskToLayer(invincibilityLayer);

        CancelInvoke(nameof(RestoreLayer));
        Invoke(nameof(RestoreLayer), invincibilityDuration);

        UpdateUI(current, max);
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
        if (move != null) move.enabled = false;

        base.ApplyKnockback(direction, force);
        Invoke(nameof(EnableMovement), 0.5f);
    }

    private void EnableMovement()
    {
        PlayerMove move = GetComponent<PlayerMove>();
        if (move != null) move.enabled = true;
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