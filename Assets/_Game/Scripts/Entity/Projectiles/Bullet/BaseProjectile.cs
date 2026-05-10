using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(TrailRenderer))]
public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected float lifeTime = 5f;
    protected LayerMask enemyLayer;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip hitSound;
    [Range(0, 10f)][SerializeField] private float hitSoundVolume;
    [SerializeField] protected AudioClip bounceSound;
    [Range(0, 10f)][SerializeField] protected float bounceSoundVolume;

    [Header("VFX")]
    public ParticleSystem bulletHitEffect;
    public ParticleSystem bulletBounceEffect;

    protected TrailRenderer trail;
    protected Vector2 direction;
    private int damageAmount;
    private int knockbackForce = 5;

    private float _lifeTimer;

    protected BoxCollider2D boxCollider;

    protected virtual void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void Setup(Vector2 launchDirection, int damage)
    {
        direction = launchDirection;
        damageAmount = damage;
        _lifeTimer = lifeTime;

        if (trail != null)
        {
            trail.Clear();
            trail.enabled = true;
        }
    }

    protected virtual void Update()
    {
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0f)
        {
            ReturnOrDestroy();
            return;
        }

        Move();
    }

    protected RaycastHit2D BoxCastForward(LayerMask mask)
    {
        Vector2 size = boxCollider != null ? boxCollider.size : Vector2.one * 0.1f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float distance = moveSpeed * Time.deltaTime;

        return Physics2D.BoxCast(
            origin: (Vector2)transform.position,
            size: size,
            angle: angle,
            direction: direction,
            distance: distance,
            layerMask: mask
        );
    }

    protected virtual void HandleHit(Collider2D other)
    {
        Health target = other.GetComponent<Health>();

        if (target != null)
        {
            target.TakeDamage(damageAmount);
            target.ApplyKnockback(direction, knockbackForce);
        }

        if (bulletHitEffect != null)
            Instantiate(bulletHitEffect, transform.position, Quaternion.identity);

        if (SfxPlayer.Instance != null)
            SfxPlayer.Instance.PlayEnvironmentSfx(clip: hitSound, volume: hitSoundVolume, loop: false);

        ReturnOrDestroy();
    }

    protected void SpawnBounceEffect()
    {
        if (bulletBounceEffect != null)
            Instantiate(bulletBounceEffect, transform.position, Quaternion.identity);
    }

    protected void ReturnOrDestroy()
    {
        if (trail != null) trail.enabled = false;

        PooledObject pooled = GetComponent<PooledObject>();

        if (pooled != null)
            pooled.ReturnToPool();
        else
            Destroy(gameObject);
    }

    protected abstract void Move();

    public void SetEnemyLayer(LayerMask layer) => enemyLayer = layer;
}