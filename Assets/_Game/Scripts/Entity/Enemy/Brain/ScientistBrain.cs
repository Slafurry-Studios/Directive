using UnityEngine;

public class ScientistBrain : EnemyBrain
{
    [Header("Flee Settings")]
    [SerializeField] private float safeDistance = 8f;
    [SerializeField] private float tolerance = 1.5f;

    [Header("Shoot Settings")]
    [SerializeField] private float shootRange = 4f;

    [Header("Stuck Detection")]
    [SerializeField] private float stuckCheckTime = 0.5f;
    [SerializeField] private float stuckDistanceThreshold = 0.1f;

    private Vector2 _lastPosition;
    private float _stuckTimer;
    private bool _isStuck;

    protected override void ExecuteBehavior()
    {
        float dist = _sensor.PlayerDistance;

        Vector2 currentPos = transform.position;

        Vector2 dirToPlayer =
            (_sensor.PlayerPos - currentPos).normalized;

        Vector2 dirAwayFromPlayer = -dirToPlayer;

        // Selalu hadap player
        _moveController.RotateTo(_sensor.PlayerPos);

        bool shouldFlee = dist < safeDistance - tolerance;

        // =========================
        // STUCK CHECK
        // =========================

        _stuckTimer += Time.deltaTime;

        if (_stuckTimer >= stuckCheckTime)
        {
            float movedDistance =
                Vector2.Distance(currentPos, _lastPosition);

            _isStuck = movedDistance < stuckDistanceThreshold;

            _lastPosition = currentPos;
            _stuckTimer = 0f;
        }


        if (shouldFlee && !_isStuck)
        {
            Vector2 moveTarget =
                currentPos + dirAwayFromPlayer * safeDistance;

            _moveController.MoveTo(moveTarget);
        }
        else
        {
            _moveController.Stop();
        }

        if (dist <= shootRange && _shootController.IsReadyToShoot())
        {
            _shootController.RequestAttack(dirToPlayer);
        }
    }
}