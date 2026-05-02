using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;
    private PlayerEnergy playerEnergy;

    [Header("Dash Physics")]
    [Tooltip("The velocity applied to the player during the dash.")]
    [SerializeField] private float dashSpeed = 20f;
    
    [Tooltip("How long the dash lasts in seconds.")]
    [SerializeField] private float dashDuration = 0.2f;
    
    [Header("Requirements and Limits")]
    [Tooltip("Time in seconds between consecutive dashes.")]
    [SerializeField] private float dashCooldown = 1f;
    
    [Tooltip("Amount of energy required to perform a dash.")]
    [SerializeField] private int dashCost = 20;

    private bool isDashing;
    private float lastDashTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerEnergy = GetComponent<PlayerEnergy>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanDash())
        {
            player.OnDashStart();
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        lastDashTime = Time.time;
        
        playerEnergy.UseEnergy(dashCost);

        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        if (inputDirection == Vector2.zero)
        {
            inputDirection = new Vector2(transform.localScale.x, 0).normalized;
        }

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = inputDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
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