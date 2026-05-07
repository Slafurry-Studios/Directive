using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float angleOffset = 0f;

    private Rigidbody2D _rb;
    private Vector2 _targetPosition;
    private bool _isMoving = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        _rb.angularDrag = 0;
    }

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            ApplyMovement();
            ApplyRotation();
        }
    }

    public void MoveTo(Vector2 target)
    {
        _targetPosition = target;
        _isMoving = true;
    }

    public void Stop()
    {
        _isMoving = false;
        _rb.velocity = Vector2.zero;
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    private void ApplyMovement()
    {
        Vector2 currentPos = _rb.position;
        float distance = Vector2.Distance(currentPos, _targetPosition);

        if (distance > 0.05f)
        {
            Vector2 direction = (_targetPosition - currentPos).normalized;
            Vector2 newPos = currentPos + direction * moveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(newPos);
        }
        else
        {
            Stop();
        }
    }

    private void ApplyRotation()
    {
        Vector2 direction = _targetPosition - _rb.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
            float smoothedAngle = Mathf.MoveTowardsAngle(_rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
            
            _rb.MoveRotation(smoothedAngle);
        }
    }
}