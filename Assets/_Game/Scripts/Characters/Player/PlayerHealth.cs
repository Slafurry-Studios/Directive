using UnityEngine;
using UnityEngine.UI; // for Slider
using UnityEngine.SceneManagement; // for SceneManager

public class PlayerHealth : Health
{
    [Header("UI Settings")]
    [SerializeField] private Slider healthSlider; 

    protected override void Start()
    {
        base.Start();
        OnHealthChanged += UpdateUI;
        OnDeath += HandlePlayerDeath;

        // Initialize UI on startup 
        UpdateUI(currentHealth, maxHealth);
    }
    private void UpdateUI(int current, int max)
    {
        // Logic for updating the player's life bar
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

        // Option 1: Immediately restart the level after the player dies 
        RestartLevel(); 

        // Option 2: Bring up the Game Over Panel (If there is a UI Panel) 
        // gameOverPanel.SetActive(true);
    }

    private void RestartLevel()
    {
        // Takes the current scene index and reloads it
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    public override void TakeDamage(int amount)
    {
        // Calls the basic function to reduce blood
        base.TakeDamage(amount);
    }

}