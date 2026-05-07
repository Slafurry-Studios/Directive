using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;

    private Enemy _enemy;
    private PatternSpawner _spawner;
    private bool _canAttack = true;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _spawner = GetComponent<PatternSpawner>();

        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    public void RequestAttack(Vector2 direction)
    {
        if (_canAttack)
        {
            StartCoroutine(AttackRoutine(direction));
        }
    }

    private IEnumerator AttackRoutine(Vector2 direction)
    {
        _canAttack = false;

        _spawner.ExecutePattern(_enemy.Info.damage, direction, firePoint.transform);

        yield return new WaitForSeconds(_enemy.Info.attackCoolDown);
        
        _canAttack = true;
    }

    public bool IsReadyToShoot() => _canAttack;
}