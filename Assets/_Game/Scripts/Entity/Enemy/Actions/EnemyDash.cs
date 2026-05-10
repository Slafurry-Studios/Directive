using System.Collections;
using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private Enemy _enemy;
    private bool isDashing;
    private float lastDashTime;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip dashSound;
    [Range(0, 10f)]
    [SerializeField] private float dashSoundVolume;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        _enemy = GetComponentInParent<Enemy>();
    }

    public void RequestDash(Vector2 direction)
    {
        if (CanDash())
        {
            _enemy.OnDashStart();
            StartCoroutine(DashRoutine(direction));
            _enemy.BodyAnimator.SetBool("Dash", isDashing);
        }
    }

    private IEnumerator DashRoutine(Vector2 direction)
    {
        isDashing = true;
        lastDashTime = Time.time;

        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayEnemySfx(clip: dashSound, volume: dashSoundVolume, loop: false);

        Quaternion originalRotation = transform.rotation;
        float originalGravity = rb.gravityScale;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        rb.gravityScale = 0f;
        rb.velocity = direction.normalized * _enemy.Info.dash.dashSpeed;

        yield return new WaitForSeconds(_enemy.Info.dash.dashDuration);

        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero;
        transform.rotation = originalRotation;

        isDashing = false;
        _enemy.ResetCondition();
    }

    public bool CanDash()
    {
        return !isDashing && Time.time >= lastDashTime + _enemy.Info.dash.dashCooldown;
    }

    public bool IsDashing => isDashing;
}