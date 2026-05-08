using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TrailRenderer))]
public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField]
    [Tooltip("How fast the projectile travels.")]
    protected float moveSpeed = 10f;

    [SerializeField]
    [Tooltip("How long the projectile lives before being destroyed.")]
    protected float lifeTime = 5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip hitSound;
    [Range(0, 10f)]
    [SerializeField] private float hitSoundVolume;
    [SerializeField] protected AudioClip bounceSound;
    [Range(0, 10f)]
    [SerializeField] protected float bounceSoundVolume;

    [Header("VFX")]
    public ParticleSystem bulletHitEffect;

    protected TrailRenderer trail;


    protected Vector2 direction;
    private int damageAmount;
    private int knockbackForce = 5;

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

            target.ApplyKnockback(direction, knockbackForce);

            Debug.Log("Player hit Enemy! Damage: " + damageAmount);
        }

        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayEnemySfx(clip: hitSound, volume: hitSoundVolume, loop: false);
        Destroy(gameObject);
    }

    protected abstract void Move();

    protected virtual void Update()
    {
        Move();
    }
}