using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] [Tooltip("How fast the projectile travels.")]
    protected float moveSpeed = 10f;

    [SerializeField] [Tooltip("How long the projectile lives before being destroyed.")]
    protected float lifeTime;

    protected Vector2 direction;

    public virtual void Setup(Vector2 launchDirection)
    {
        direction = launchDirection;
        Destroy(gameObject, lifeTime);
    }

    protected abstract void Move();

    protected virtual void Update()
    {
        Move();
    }
}