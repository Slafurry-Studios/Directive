using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float angleOffset = 0f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip moveSound;

    [Range(0f, 10f)]
    [SerializeField] private float moveSoundVolume = 1f;

    private Enemy _data;
    private Rigidbody2D _rb;

    private Vector2 _targetPosition;
    private bool _isMoving = false;
    private bool _wasMoving = false;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _data = GetComponentInParent<Enemy>();

        _rb.gravityScale = 0f;
        _rb.angularDrag = 0f;
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

        if (!_isMoving)
        {
            _isMoving = true;
            StartMoveSfx();
        }
    }

    public void Stop()
    {
        if (!_isMoving) return;

        _isMoving = false;
        _rb.velocity = Vector2.zero;

        StopMoveSfx();
    }

    public void RotateTo(Vector2 target)
    {
        Vector2 direction = target - _rb.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        float targetAngle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;

        float smoothedAngle =
            Mathf.MoveTowardsAngle(
                _rb.rotation,
                targetAngle,
                _data.Info.movement.rotationSpeed * Time.fixedDeltaTime
            );

        _rb.MoveRotation(smoothedAngle);
    }

    private void ApplyMovement()
    {
        Vector2 currentPos = _rb.position;

        float distance =
            Vector2.Distance(currentPos, _targetPosition);

        if (distance > 0.05f)
        {
            Vector2 direction =
                (_targetPosition - currentPos).normalized;

            Vector2 newPos =
                currentPos +
                direction *
                _data.Info.movement.moveSpeed *
                Time.fixedDeltaTime;

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
            float targetAngle =
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;

            float smoothedAngle =
                Mathf.MoveTowardsAngle(
                    _rb.rotation,
                    targetAngle,
                    _data.Info.movement.rotationSpeed * Time.fixedDeltaTime
                );

            _rb.MoveRotation(smoothedAngle);
        }
    }

    private void StartMoveSfx()
    {
        if (SfxPlayer.Instance == null) return;

        if (_wasMoving) return;

        SfxPlayer.Instance.PlayEnemySfx(
            clip: moveSound,
            volume: moveSoundVolume,
            loop: true
        );

        _wasMoving = true;
    }

    private void StopMoveSfx()
    {
        if (SfxPlayer.Instance == null) return;

        if (!_wasMoving) return;

        SfxPlayer.Instance.StopLoopingSfx(moveSound);

        _wasMoving = false;
    }
}