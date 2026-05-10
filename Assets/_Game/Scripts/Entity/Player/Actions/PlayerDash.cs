using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;
    private PlayerEnergy playerEnergy;

    [Header("Dash Physics")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private LayerMask invincibilityLayer;
    private int originalLayer;

    [Header("Requirements Limits")]
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private int dashCost = 20;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip dashSound;
    [Range(0, 10f)]
    [SerializeField] private float dashSoundVolume;

    private bool isDashing;
    private float lastDashTime;
    private Quaternion originalRotation;
    private Vector2 lastMoveDirection;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerEnergy = GetComponent<PlayerEnergy>();
        animator = GetComponent<Animator>();
        originalLayer = gameObject.layer;

    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput.sqrMagnitude > 1)
        {

            moveInput.Normalize();
        }
        if (moveInput.sqrMagnitude > 0)
        {
            lastMoveDirection = moveInput;
        }
        if (Input.GetKeyDown(KeyCode.Space) && CanDash())
        {
            player.OnDashStart();
            StartCoroutine(DashRoutine());
            animator.SetBool("Dash", isDashing);
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        lastDashTime = Time.time;
        originalRotation = transform.rotation;
        gameObject.layer = MaskToLayer(invincibilityLayer);

        playerEnergy.UseEnergy(dashCost);
        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayPlayerSfx(clip: dashSound, volume: dashSoundVolume, loop: false);

        float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = lastMoveDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        transform.rotation = originalRotation;

        isDashing = false;
        player.ResetCondition();
        RestoreLayer();
    }

    private bool CanDash()
    {
        return !isDashing &&
               playerEnergy.CurrentEnergy >= dashCost &&
               Time.time >= lastDashTime + dashCooldown;
    }

    private void RestoreLayer()
    {
        gameObject.layer = originalLayer;
    }

    private int MaskToLayer(LayerMask mask)
    {
        int bitmask = mask.value;
        if (bitmask == 0) return 0;
        int result = 0;
        while (bitmask > 1)
        {
            bitmask >>= 1;
            result++;
        }
        return result;
    }
}