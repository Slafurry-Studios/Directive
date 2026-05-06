using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField]
    [Tooltip("How fast the projectile travels.")]
    protected float moveSpeed = 10f;

    [SerializeField]
    [Tooltip("How long the projectile lives before being destroyed.")]
    protected float lifeTime = 5f;

    protected Vector2 direction;
    protected int damageAmount = 10;

    public virtual void Setup(Vector2 launchDirection, int damage)
    {
        direction = launchDirection;
        damageAmount = damage;
        Destroy(gameObject, lifeTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Health target = other.GetComponent<Health>();

        if (target != null)
        {
            target.TakeDamage(damageAmount);

            Debug.Log("Player hit Enemy! Damage: " + damageAmount);
        }

        Destroy(gameObject);
    }

    protected abstract void Move();

    protected virtual void Update()
    {
        Move();
    }
}