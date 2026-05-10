using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(TrailRenderer))]
public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Base Settings")]
    protected float moveSpeed = 10f;
    [SerializeField] protected float lifeTime = 5f;
    protected LayerMask enemyLayer;
    [SerializeField] protected LayerMask environmentLayer;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip hitSound;
    [Range(0, 10f)][SerializeField] private float hitSoundVolume;

    [Header("VFX")]
    public ParticleSystem bulletHitEffect;

    [Header("Shrink Settings")]
    [SerializeField] private bool enableShrink = false;
    [SerializeField] private float initialScale = 1f;
    [SerializeField] private float minimumScale = 0.1f;
    [SerializeField] private float shrinkSpeed = 0.3f;

    protected TrailRenderer trail;
    protected Vector2 direction;
    private int damageAmount;
    private int knockbackForce = 5;

    private float _lifeTimer;
    private float _currentScale;
    private bool _isShrinking = false;

    protected BoxCollider2D boxCollider;
    protected virtual bool HandleEnvironmentInUpdate => true;

    protected virtual void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void Setup(Vector2 launchDirection, int damage, int speed)
    {
        direction = launchDirection;
        damageAmount = damage;
        moveSpeed = speed;
        _lifeTimer = lifeTime;

        if (enableShrink)
        {
            _currentScale = initialScale;
            _isShrinking = false;
            transform.localScale = Vector3.one * _currentScale;
        }

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

        if (HandleEnvironmentInUpdate)
        {
            RaycastHit2D envHit = BoxCastForward(environmentLayer);
            if (envHit.collider != null)
            {
                HandleEnvironmentHit();
                return;
            }
        }

        Move();

        if (enableShrink && _isShrinking)
            UpdateShrink();
    }

    protected virtual void HandleEnvironmentHit()
    {
        if (bulletHitEffect != null)
            Instantiate(bulletHitEffect, transform.position, Quaternion.identity);

        ReturnOrDestroy();
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
        Health target = other.GetComponentInChildren<Health>();

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

    protected void StartShrinking()
    {
        if (enableShrink)
            _isShrinking = true;
    }

    private void UpdateShrink()
    {
        _currentScale -= shrinkSpeed * Time.deltaTime;
        _currentScale = Mathf.Max(_currentScale, minimumScale);
        transform.localScale = Vector3.one * _currentScale;

        if (_currentScale <= minimumScale)
            ReturnOrDestroy();
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