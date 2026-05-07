using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [Header("UI Settings")]
    [SerializeField] public Image healthBar;
    [SerializeField] public Image[] healthPoints;

    private Animator animator;
    private float lerpSpeed;

    protected override void Start()
    {
        base.Start();
        OnHealthChanged += UpdateUI;
        OnDeath += HandlePlayerDeath;
        animator = GetComponent<Animator>();

        UpdateUI(currentHealth, maxHealth);
    }

    private void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
        HealthBarFiller();
    }

    private void UpdateUI(int current, int max)
    {
        animator.SetTrigger("onHit");
        Debug.Log($"Player Health: {current} / {max}");
    }

    private void HealthBarFiller()
    {
        float healthRatio = (float)currentHealth / maxHealth;

        if (healthBar != null)
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, healthRatio, lerpSpeed);

        for (int i = 0; i < healthPoints.Length; i++)
        {
            healthPoints[i].enabled = DisplayHealthPoint(currentHealth, i);
        }
    }

    private bool DisplayHealthPoint(float _health, int pointNumber)
    {
        return ((pointNumber * 10) < _health);
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