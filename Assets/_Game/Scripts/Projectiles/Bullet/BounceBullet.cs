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

    private Rigidbody2D rb;
    private Collider2D myCollider;
    private int currentBounceCount;
    private float currentScale;
    private bool hasBounced;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        trail = GetComponent<TrailRenderer>();

        rb.freezeRotation = true;
        if (myCollider != null) myCollider.isTrigger = true;
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
        rb.velocity = direction * moveSpeed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & environmentLayer) != 0)
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, environmentLayer);

            if (hit.collider != null)
            {
                Reflect(hit.normal);
            }
            else
            {
                Vector2 stickyNormal = ((Vector2)transform.position - other.ClosestPoint(transform.position)).normalized;
                Reflect(stickyNormal);

            }
        }
        else
        {
            base.OnTriggerEnter2D(other);
        }
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

        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayEnemySfx(clip: bounceSound, volume: bounceSoundVolume, loop: false);

        direction = Vector2.Reflect(direction, normal).normalized;
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