using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    [SerializeField] private GameObject healthBarPrefab;
    private Enemy enemy;
    private Animator animator;
    private EnemyHealthBar healthBar;
    private bool isHealthShown = false;
    protected override void Start()
    {
        base.Start();

        OnHealthChanged += HandleHit;
        OnDeath += Death;

        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        maxHealth = enemy.Info.stats.maxHealth;
        currentHealth = maxHealth;
    }

    private void HandleHit(int current, int max)
    {
        if (IsDead) return;
        animator.SetTrigger("onHit");
        UpdateUI(current, max);

        if (HitPause.Instance != null)
            HitPause.Instance.Pause(0.05f);
    }

    public override void ApplyKnockback(Vector2 direction, float force)
    {
        EnemyMovement move = GetComponent<EnemyMovement>();
        if (move != null) move.enabled = false;

        base.ApplyKnockback(direction, force);
        Invoke(nameof(EnableMovement), 0.5f);
    }

    private void UpdateUI(int current, int max)
    {

        if (!isHealthShown)
        {
            GameObject healthBarObj = Instantiate(healthBarPrefab);
            healthBar = healthBarObj.GetComponent<EnemyHealthBar>();
            healthBar.SetTarget(transform);
            isHealthShown = true;
        }

        healthBar.SetHealth(current, max);
    }

    private void EnableMovement()
    {
        EnemyMovement move = GetComponent<EnemyMovement>();
        if (move != null) move.enabled = true;
    }


    protected override void Death()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;

        animator.SetBool("isDead", true);
        enemy.Collection.RemoveEnemy(enemy);
        EnemyIndicatorManager.Instance.UnregisterEnemy(transform);

        Destroy(healthBar.gameObject);
        Destroy(gameObject, 2f);
    }
}