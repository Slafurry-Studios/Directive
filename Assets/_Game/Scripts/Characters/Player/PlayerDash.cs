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

        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        if (inputDirection == Vector2.zero)
        {
            inputDirection = new Vector2(transform.localScale.x, 0).normalized;
        }

        float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = inputDirection * dashSpeed;

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