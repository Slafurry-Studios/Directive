using UnityEngine;

public class BounceBullet : BaseProjectile
{
    [Header("Bounce Settings")]
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private int maxBounces = 3;

    [Header("Shrink Settings")]
    [SerializeField] private float initialScale = 1f;
    [SerializeField] private float minimumScale = 0.1f;
    [SerializeField] private float shrinkSpeed = 0.3f;

    private int currentBounceCount;
    private float currentScale;
    private bool hasBounced;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Setup(Vector2 launchDirection, int damage)
    {
        base.Setup(launchDirection, damage);
        currentBounceCount = 0;
        currentScale = initialScale;
        hasBounced = false;
        transform.localScale = Vector3.one * currentScale;
        direction = launchDirection.normalized;
    }

    protected override void Update()
    {
        base.Update();

        if (hasBounced)
            ShrinkOverTime();
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
        hasBounced = true;

        if (currentBounceCount > maxBounces)
        {
            ReturnOrDestroy();
            return;
        }

        if (SfxPlayer.Instance != null)
            SfxPlayer.Instance.PlayEnvironmentSfx(clip: bounceSound, volume: bounceSoundVolume, loop: false);

        direction = Vector2.Reflect(direction, normal).normalized;
        transform.Translate(direction * 0.05f, Space.World);
    }

    private void ShrinkOverTime()
    {
        currentScale -= shrinkSpeed * Time.deltaTime;
        currentScale = Mathf.Max(currentScale, minimumScale);
        transform.localScale = Vector3.one * currentScale;

        if (currentScale <= minimumScale)
            ReturnOrDestroy();
    }
}