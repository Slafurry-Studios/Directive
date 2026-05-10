using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 15f;

    [SerializeField] private Animator feetAnimator;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip moveSound;

    [Range(0f, 10f)]
    [SerializeField] private float moveSoundVolume = 1f;

    private Player player;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection;
    private Animator bodyAnim;

    private bool wasMoving;

    void Start()
    {
        player = GetComponentInParent<Player>();
        
        bodyAnim = player.bodyAnim;
        rb = player.rb;
    }

    void Update()
    {
        ReadInput();

        bodyAnim.SetBool("isAiming", player.IsAiming());
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

        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput.Normalize();
        }

        bool isMoving = moveInput.sqrMagnitude > 0f;

        if (isMoving)
        {
            lastMoveDirection = moveInput;
        }

        HandleMoveSfx(isMoving);

        bodyAnim.SetFloat("move", moveInput.sqrMagnitude);
        feetAnimator.SetFloat("move", moveInput.sqrMagnitude);
    }

    private void Move()
    {
        Vector2 nextPosition =
            rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);
    }

    private void HandleRotation()
    {
        if (moveInput.sqrMagnitude > 0f)
        {
            float targetAngle =
                Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

            Quaternion targetRotation =
                Quaternion.Euler(0f, 0f, targetAngle + 90f);

            transform.parent.rotation = Quaternion.RotateTowards(
                transform.parent.rotation,
                targetRotation,
                rotationSpeed * 360f * Time.fixedDeltaTime
            );
        }
    }

    private void HandleMoveSfx(bool isMoving)
    {
        if (SfxPlayer.Instance == null) return;

        if (isMoving && !wasMoving)
        {
            SfxPlayer.Instance.PlayPlayerSfx(
                clip: moveSound,
                volume: moveSoundVolume,
                loop: true
            );
        }

        if (!isMoving && wasMoving)
        {
            SfxPlayer.Instance.StopLoopingSfx(moveSound);
        }

        wasMoving = isMoving;
    }
}