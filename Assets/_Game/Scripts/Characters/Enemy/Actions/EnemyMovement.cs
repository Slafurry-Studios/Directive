using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float angleOffset = 0f;
    private Enemy _data;
    private Rigidbody2D _rb;
    private Vector2 _targetPosition;
    private bool _isMoving = false;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _data = GetComponentInParent<Enemy>();
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

    private void ApplyMovement()
    {
        Vector2 currentPos = _rb.position;
        float distance = Vector2.Distance(currentPos, _targetPosition);

        if (distance > 0.05f)
        {
            Vector2 direction = (_targetPosition - currentPos).normalized;
            Vector2 newPos = currentPos + direction * _data.Info.movement.moveSpeed * Time.fixedDeltaTime;
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
            float smoothedAngle = Mathf.MoveTowardsAngle(_rb.rotation, targetAngle, _data.Info.movement.rotationSpeed * Time.fixedDeltaTime);
            
            _rb.MoveRotation(smoothedAngle);
        }
    }
}