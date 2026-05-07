using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    private EnemyMovement _moveController;
    private EnemyShoot _shootController;
    private EnemySensor _sensor;
    private Enemy _enemy;

    private void Start()
    {
        _moveController = GetComponent<EnemyMovement>();
        _shootController = GetComponent<EnemyShoot>();
        _sensor = GetComponent<EnemySensor>();
        _enemy = GetComponent<Enemy>();


    }

    private void Update()
    {
        HandleMovement();
        HandleCombat();
    }

    private void HandleMovement()
    {
        if (_sensor.PlayerDistance > 1.5f)
        {
            _moveController.MoveTo(_sensor.PlayerPos);
        }
        else
        {
            _moveController.Stop();
        }
    }

    private void HandleCombat()
    {
        if (_sensor.IsPlayerInAttackCone)
        {
            Vector2 direction = CalculateAttackDirection();
            _shootController.RequestAttack(direction);
        }
    }

    private Vector2 CalculateAttackDirection()
    {
        Vector2 myPos = (Vector2)transform.position;
        Vector2 targetPos = _sensor.PlayerPos;

        Vector2 baseDirection = (targetPos - myPos).normalized;

        return baseDirection;
    }
}