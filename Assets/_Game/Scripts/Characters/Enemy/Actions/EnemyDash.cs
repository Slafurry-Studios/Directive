using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy _enemy; 

    [Header("Dash Physics")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing;
    private float lastDashTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Enemy>();
    }

    public void RequestDash(Vector2 direction)
    {
        if (CanDash())
        {
            _enemy.OnDashStart();
            StartCoroutine(DashRoutine(direction));
        }
    }

    private IEnumerator DashRoutine(Vector2 direction)
    {
        isDashing = true;
        lastDashTime = Time.time;

        Quaternion originalRotation = transform.rotation;
        float originalGravity = rb.gravityScale;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        rb.gravityScale = 0f;
        rb.velocity = direction.normalized * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero;
        transform.rotation = originalRotation;

        isDashing = false;
        _enemy.ResetCondition();
    }

    public bool CanDash()
    {
        return !isDashing && Time.time >= lastDashTime + dashCooldown;
    }

    public bool IsDashing => isDashing;
}