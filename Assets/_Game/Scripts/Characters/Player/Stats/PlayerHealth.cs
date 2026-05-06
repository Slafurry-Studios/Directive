using UnityEngine;
using UnityEngine.UI; // for Slider
using UnityEngine.SceneManagement; // for SceneManager

public class PlayerHealth : Health
{
    [Header("UI Settings")]
    [SerializeField] private Slider healthSlider; 
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        OnHealthChanged += UpdateUI;
        OnDeath += HandlePlayerDeath;
        animator = GetComponent<Animator>();

        // Initialize UI on startup 
        UpdateUI(currentHealth, maxHealth);
    }
    private void UpdateUI(int current, int max)
    {
        animator.SetTrigger("onHit");
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
        
        Debug.Log($"Player Health: {current} / {max}");
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player died! Show Game Over.");

        RestartLevel(); 

    }

    public override void ApplyKnockback(Vector2 direction, float force)
    {
        PlayerMove move = GetComponent<PlayerMove>();
        move.enabled = false;
        base.ApplyKnockback(direction, force);

        // Re-enable movement after a short delay (e.g., 0.5 seconds)
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
        // Takesthe current scene index and reloads it
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

}