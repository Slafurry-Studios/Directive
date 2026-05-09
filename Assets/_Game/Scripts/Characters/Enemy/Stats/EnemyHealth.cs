using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private GameObject deathVFX;
    private Enemy enemy;
    private Animator animator;
    protected override void Start()
    {
        base.Start();
        OnHealthChanged += HandleHit;
        OnDeath += Death;
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

    }

    private void HandleHit(int current, int max)
    {
        if (IsDead) return;
        animator.SetTrigger("onHit");
    }

    public override void ApplyKnockback(Vector2 direction, float force)
    {
        EnemyMovement move = GetComponent<EnemyMovement>();
        if (move != null) move.enabled = false;

        base.ApplyKnockback(direction, force);
        Invoke(nameof(EnableMovement), 0.5f);
    }

    private void EnableMovement()
    {
        EnemyMovement move = GetComponent<EnemyMovement>();
        if (move != null) move.enabled = true;
    }


    protected override void Death()
    {
        if (deathVFX != null) Instantiate(deathVFX, transform.position, Quaternion.identity);
        animator.SetBool("isDead", true);
        enemy.Collection.RemoveEnemy(enemy);
        Destroy(gameObject, 2f);
    }
}