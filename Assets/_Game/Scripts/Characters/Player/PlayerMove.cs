using UnityEngine;

public class PlayerMove : MonoBehaviour
{
<<<<<<< HEAD
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f; // Slerp needs a lower value for smooth feel (e.g., 10-20)

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;

    void Awake()
    {
        // Based on existing scripts, rb is in parent
        rb = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        ReadInput();
        HandleRotation();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void ReadInput()
    {
        // Phase 2: read input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        moveInput = new Vector2(moveX, moveY);
        
        // Normalization for consistent diagonal speed
        if (moveInput.sqrMagnitude > 1)
        {
            moveInput.Normalize();
        }

        if (moveInput.sqrMagnitude > 0)
        {
            lastMoveDirection = moveInput;
        }
    }

    private void Move()
    {
        // Phase 2: Execute movement using Rigidbody2D MovePosition
        // Using MovePosition + deltaTime provides more precise physical movement control
        Vector2 nextPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);
    }

    private void HandleRotation()
    {
        // Phase 3: Rotation Logic (Directional Facing)
        // Only rotate if the player is moving
        if (moveInput.sqrMagnitude > 0)
        {
            // Calculate the target angle based on movement input
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            
            // aUsing Slerp for organic smoothing (accelerates and decelerates towards target)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
=======
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }


>>>>>>> d87c479b391dbd53a5351895cf634a87b14dc689
}