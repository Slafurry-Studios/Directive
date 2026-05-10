using UnityEngine;

public abstract class EnemyBrain : MonoBehaviour
{
    protected EnemyMovement _moveController;
    protected EnemyShoot _shootController;
    protected EnemySensor _sensor;
    protected EnemyDash _dash;
    protected Enemy _enemyInfo;
    private EnemyHealth _enemyHealth;

    protected virtual void Awake()
    {
        _moveController = GetComponentInChildren<EnemyMovement>();
        _shootController = GetComponentInChildren<EnemyShoot>();
        _sensor = GetComponentInChildren<EnemySensor>();
        _dash = GetComponentInChildren<EnemyDash>();
        _enemyInfo = GetComponent<Enemy>();
        _enemyHealth = GetComponentInChildren<EnemyHealth>();
    }

    protected virtual void Update()
    {
        if (_enemyInfo.Target == null) return;

        if (_enemyHealth.IsDead) return;
        ExecuteBehavior();
    }

    protected abstract void ExecuteBehavior();
}