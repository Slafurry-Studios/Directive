using UnityEngine;

public class BounceBullet : BaseProjectile
{
    [Header("Bounce Settings")]
    [SerializeField]
    private LayerMask environmentLayer;

    [SerializeField]
    private int maxBounces = 3;

    [Header("Shrink Settings")]
    [SerializeField]
    private float initialScale = 1f;

    [SerializeField]
    private float minimumScale = 0.1f;

    [SerializeField]
    private float shrinkSpeed = 0.3f;

    private Rigidbody2D rb;
    private int currentBounceCount;
    private float currentScale;
    private bool hasBounced;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // penting: hindari rotasi dari physics
        rb.freezeRotation = true;
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

        Move();

        if (hasBounced)
            ShrinkOverTime();
    }

    protected override void Move()
    {
        rb.velocity = direction * moveSpeed;

        // rotate mengikuti arah gerak (bukan physics)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // ====== AKURAT: WAJIB COLLISION ======
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & environmentLayer) != 0)
        {
            ContactPoint2D contact = collision.GetContact(0);

            Reflect(contact.normal);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Reflect(Vector2 normal)
    {
        currentBounceCount++;
        hasBounced = true;

        if (currentBounceCount >= maxBounces)
        {
            Destroy(gameObject);
            return;
        }

        // hukum refleksi: sudut datang = sudut pantul
        direction = Vector2.Reflect(direction, normal).normalized;
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