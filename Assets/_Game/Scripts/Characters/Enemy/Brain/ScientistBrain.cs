using UnityEngine;

public class ScientistBrain : EnemyBrain
{
    [Header("Flee Settings")]
    [SerializeField] private float safeDistance = 8f;
    [SerializeField] private float tolerance = 1.5f;

    [Header("Shoot Settings")]
    [SerializeField] private float shootRange = 4f;

    protected override void ExecuteBehavior()
    {
        float dist = _sensor.PlayerDistance;
        Vector2 dirAwayFromPlayer = ((Vector2)transform.position - _sensor.PlayerPos).normalized;

        _moveController.RotateTo(_sensor.PlayerPos);

        if (dist < safeDistance - tolerance)
        {
            Vector2 fleeTarget = (Vector2)transform.position + dirAwayFromPlayer * safeDistance;
            _moveController.MoveTo(fleeTarget);
        }
        else
        {
            _moveController.Stop();
        }

        if (dist <= shootRange && _shootController.IsReadyToShoot())
        {
            Vector2 dirToPlayer = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
            _shootController.RequestAttack(dirToPlayer);
        }
    }
}