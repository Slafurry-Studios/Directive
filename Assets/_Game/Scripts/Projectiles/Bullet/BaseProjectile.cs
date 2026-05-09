using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TrailRenderer))]
public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected float lifeTime = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip hitSound;
    [Range(0, 10f)][SerializeField] private float hitSoundVolume;
    [SerializeField] protected AudioClip bounceSound;
    [Range(0, 10f)][SerializeField] protected float bounceSoundVolume;

    [Header("VFX")]
    public ParticleSystem bulletHitEffect;

    protected TrailRenderer trail;
    protected Vector2 direction;
    private int damageAmount;
    private int knockbackForce = 5;

    private PooledObject _pooled;
    private float _lifeTimer;

    protected virtual void Awake()
    {
        _pooled = GetComponent<PooledObject>();
        trail = GetComponent<TrailRenderer>();
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

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Health target = other.GetComponent<Health>();

        if (target != null)
        {
            target.TakeDamage(damageAmount);
            target.ApplyKnockback(direction, knockbackForce);
        }

        if (SfxPlayer.Instance != null)
            SfxPlayer.Instance.PlayEnemySfx(clip: hitSound, volume: hitSoundVolume, loop: false);

        ReturnOrDestroy();
    }

    protected void ReturnOrDestroy()
    {
        if (trail != null) trail.enabled = false;

        if (_pooled != null)
            _pooled.ReturnToPool();
        else
            Destroy(gameObject);
    }

    protected abstract void Move();
}