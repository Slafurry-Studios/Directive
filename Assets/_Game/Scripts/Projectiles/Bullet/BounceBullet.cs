using UnityEngine;

public class BounceBullet : BaseProjectile
{
    // ============ VARIABLES ============

    [Header("Bounce Settings")]

    [SerializeField]
    [Tooltip("Which physics layers count as Environment surfaces the bullet can bounce off.")]
    private LayerMask environmentLayer;

    [SerializeField]
    [Tooltip("Maximum number of times the bullet can bounce before being destroyed.")]
    private int maxBounces = 3;

    [Header("Shrink Settings")]

    [SerializeField]
    [Tooltip("The scale the bullet starts at when first fired.")]
    private float initialScale = 1f;

    [SerializeField]
    [Tooltip("The minimum scale the bullet shrinks down to before being destroyed.")]
    private float minimumScale = 0.1f;

    [SerializeField]
    [Tooltip("How fast the bullet shrinks over time after the first bounce. Higher values shrink faster.")]
    private float shrinkSpeed = 0.3f;

    private Rigidbody2D rb;
    private int currentBounceCount;
    private float currentScale;
    private bool hasBounced;

    // ============ UNITY EVENTS ============

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Setup(Vector2 launchDirection, int damage)
    {
        base.Setup(launchDirection, damage);

        currentBounceCount = 0;
        currentScale = initialScale;
        hasBounced = false;
        transform.localScale = Vector3.one * currentScale;

        rb.velocity = launchDirection * moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        if (hasBounced) ShrinkOverTime();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & environmentLayer) != 0)
        {
            HandleEnvironmentBounce(collision.GetContact(0).normal);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ============ LOGIC ============

    protected override void Move()
    {
        // Movement is driven by Rigidbody2D.linearVelocity so the physics engine
        // can sweep collisions correctly. Nothing to do here each frame.
    }

    private void HandleEnvironmentBounce(Vector2 contactNormal)
    {
        currentBounceCount++;
        hasBounced = true;

        if (currentBounceCount >= maxBounces)
        {
            Destroy(gameObject);
            return;
        }

        direction = Vector2.Reflect(direction, contactNormal).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private void ShrinkOverTime()
    {
        currentScale -= shrinkSpeed * Time.deltaTime;
        currentScale = Mathf.Max(currentScale, minimumScale);
        transform.localScale = Vector3.one * currentScale;

        if (Mathf.Approximately(currentScale, minimumScale))
            Destroy(gameObject);
    }
}