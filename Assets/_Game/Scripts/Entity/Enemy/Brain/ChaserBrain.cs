using UnityEngine;

public class ChaserBrain : EnemyBrain
{
    [Header("Distance Settings")]
    [SerializeField] private float preferredDistance = 5f;
    [SerializeField] private float tolerance = 1.5f;
    [SerializeField] private float repositionInterval = 2f;

    [Header("Dodge Settings")]
    [SerializeField] private float dodgeChance = 0.4f;

    private float _repositionTimer = 0f;
    private bool _isSettled = false;

    protected override void ExecuteBehavior()
    {
        float dist = _sensor.PlayerDistance;
        float min = preferredDistance - tolerance;
        float max = preferredDistance + tolerance;

        if (!_isSettled)
        {
            if (dist < min || dist > max)
            {
                Vector2 dir = ((Vector2)transform.position - _sensor.PlayerPos).normalized;
                Vector2 targetPos = _sensor.PlayerPos + dir * preferredDistance;
                _moveController.MoveTo(targetPos);
            }
            else
            {
                _moveController.Stop();
                _isSettled = true;
                _repositionTimer = repositionInterval;
            }
        }
        else
        {
            // Always face the player while settled
            _moveController.RotateTo(_sensor.PlayerPos);

            if (_sensor.IsPlayerInAttackCone && _shootController.IsReadyToShoot())
            {
                Vector2 dir = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
                _shootController.RequestAttack(dir);
            }

            if (_dash.CanDash() && Random.value < dodgeChance * Time.deltaTime)
            {
                Vector2 dirToPlayer = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
                Vector2 side = Vector2.Perpendicular(dirToPlayer);
                if (Random.value < 0.5f) side = -side;
                _dash.RequestDash(side);
            }

            _repositionTimer -= Time.deltaTime;
            if (_repositionTimer <= 0f)
            {
                _isSettled = false;
                _repositionTimer = repositionInterval;
            }
        }
    }
}