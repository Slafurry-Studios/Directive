using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections;

public class PlayerHealth : Health
{
    [Header("VFX")]
    [SerializeField] private GameObject healVFX;

    private Player player;
    private Animator bodyAnim;
    private InvincibilityHandler invincibilityHandler;
    private OverlayEffect overlayEffect;
    protected override void Start()
    {
        base.Start();
        player = GetComponentInParent<Player>();

        squashOnHit = GetComponent<SquashOnHit>();
        invincibilityHandler = GetComponentInChildren<InvincibilityHandler>();
        overlayEffect = GetComponentInChildren<OverlayEffect>();

        OnHealthChanged += HandleHealthChanged;
        OnDeath += HandlePlayerDeath;

        bodyAnim = player.bodyAnim;

        UpdateUI(currentHealth, maxHealth);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

        bodyAnim.SetTrigger("onHit");

        invincibilityHandler.StartInvincibility();
        overlayEffect.StartDamageEffect();

    }

    private void HandleHealthChanged(int current, int max)
    {
        UpdateUI(current, max);
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        Instantiate(healVFX, transform.position, Quaternion.identity);
    }

    private void UpdateUI(int current, int max)
    {
        if (HealthHUD.Instance != null)
            HealthHUD.Instance.UpdateUI(current, max);
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
}