using UnityEngine;
using System.Collections;

public class TankBrain : EnemyBrain
{
    [Header("Distance Settings")]
    [SerializeField] private float preferredDistance = 6f;
    [SerializeField] private float tolerance = 2f;
    [SerializeField] private float repositionInterval = 3f;

    [Header("Shoot Cycle")]
    [SerializeField] private float shootInterval = 4f;

    private float _shootTimer = 0f;
    private float _repositionTimer = 0f;
    private bool _isSettled = false;
    private bool _isShooting = false;

    protected override void ExecuteBehavior()
    {
        // Always rotate toward player regardless of state
        _moveController.RotateTo(_sensor.PlayerPos);

        if (_isShooting) return;

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
            _shootTimer += Time.deltaTime;
            if (_shootTimer >= shootInterval)
            {
                _shootTimer = 0f;
                StartCoroutine(ShootRoutine());
            }

            _repositionTimer -= Time.deltaTime;
            if (_repositionTimer <= 0f)
            {
                _isSettled = false;
                _repositionTimer = repositionInterval;
            }
        }
    }

    private IEnumerator ShootRoutine()
    {
        _isShooting = true;
        _moveController.Stop();

        yield return new WaitUntil(() => _shootController.IsReadyToShoot());

        Vector2 dir = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
        _shootController.RequestAttack(dir);

        yield return new WaitUntil(() => _shootController.IsReadyToShoot());

        _isShooting = false;
    }
}