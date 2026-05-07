using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;

    [SerializeField] private Animator feetAnimator;

    private Player player;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private Animator animator;

    void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        ReadInput();
        animator.SetBool("isAiming", player.IsAiming());
    }

    void FixedUpdate()
    {
        Move();

        if (!player.IsAiming())
        {
            HandleRotation();
        }
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

        animator.SetFloat("move", moveInput.sqrMagnitude);
        feetAnimator.SetFloat("move", moveInput.sqrMagnitude);
    }

    private void Move()
    {
        float speedModifier = moveSpeed;

        Vector2 nextPosition = rb.position + moveInput * speedModifier * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);
    }

    private void HandleRotation()
    {
        if (moveInput.sqrMagnitude > 0)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle + 90f);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * 360f * Time.fixedDeltaTime
            );
        }
    }
}