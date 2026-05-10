using UnityEngine;

public class BounceBullet : BaseProjectile
{
    [Header("Bounce Settings")]
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private AudioClip bounceSound;
    [Range(0, 10f)][SerializeField] private float bounceSoundVolume;
    [SerializeField] private ParticleSystem bulletBounceEffect;

    private int currentBounceCount;

    protected override bool HandleEnvironmentInUpdate => false;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Setup(Vector2 launchDirection, int damage, int speed)
    {
        base.Setup(launchDirection, damage, speed);
        currentBounceCount = 0;
        direction = launchDirection.normalized;
    }

    protected override void Move()
    {
        RaycastHit2D envHit = BoxCastForward(environmentLayer);
        if (envHit.collider != null)
        {
            Reflect(envHit.normal);
            return;
        }

        RaycastHit2D enemyHit = BoxCastForward(enemyLayer);
        if (enemyHit.collider != null)
        {
            HandleHit(enemyHit.collider);
            return;
        }

        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Reflect(Vector2 normal)
    {
        currentBounceCount++;

        if (currentBounceCount > maxBounces)
        {
            ReturnOrDestroy();
            return;
        }

        StartShrinking();

        if (bulletBounceEffect != null)
            Instantiate(bulletBounceEffect, transform.position, Quaternion.identity);

        if (SfxPlayer.Instance != null)
            SfxPlayer.Instance.PlayBulletSfx(clip: bounceSound, volume: bounceSoundVolume, loop: false);

        direction = Vector2.Reflect(direction, normal).normalized;
        transform.Translate(direction * 0.05f, Space.World);
    }
}