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

    [Header("Requirements Limits")]
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private int dashCost = 20;

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

        playerEnergy.UseEnergy(dashCost);

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
    }

    private bool CanDash()
    {
        return !isDashing &&
               playerEnergy.CurrentEnergy >= dashCost &&
               Time.time >= lastDashTime + dashCooldown;
    }
}