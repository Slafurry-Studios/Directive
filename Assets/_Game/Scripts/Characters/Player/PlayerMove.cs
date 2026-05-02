using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;

    void Awake()
    {
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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        moveInput = new Vector2(moveX, moveY);
        
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
        Vector2 nextPosition = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);
    }

    private void HandleRotation()
    {
        if (moveInput.sqrMagnitude > 0)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}