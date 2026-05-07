using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour
{
    protected EnemyMovement _moveController;
    protected EnemyShoot _shootController;
    protected EnemySensor _sensor;
    protected EnemyDash _dash;
    protected Enemy _enemyInfo;

    protected virtual void Awake()
    {
        _moveController = GetComponentInChildren<EnemyMovement>();
        _shootController = GetComponentInChildren<EnemyShoot>();
        _sensor = GetComponentInChildren<EnemySensor>();
        _dash = GetComponentInChildren<EnemyDash>();
        _enemyInfo = GetComponent<Enemy>();
    }

    protected virtual void Update()
    {
        if (_enemyInfo.Target == null) return;
        ExecuteBehavior();
    }

    protected abstract void ExecuteBehavior();
}