using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private Enemy _enemy;
    private EnemySensor _sensor;
    private PatternSpawner _spawner;
    private bool _canAttack = true;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _spawner = GetComponent<PatternSpawner>();
        _sensor = GetComponent<EnemySensor>();
    }

    void Update()
    {
        if (PlayerInSight() && _canAttack)
        {
            Vector2 playerPos = _sensor.PlayerPos;
            Vector2 myPos = (Vector2)transform.position;
            Vector2 direction = playerPos - myPos + 90f * Vector2.Perpendicular(playerPos - myPos);

            StartCoroutine(AttackRoutine(direction));
        }
    }

    private IEnumerator AttackRoutine(Vector2 direction)
    {
        _canAttack = false;

        _spawner.ExecutePattern(_enemy.Info.damage);

        yield return new WaitForSeconds(_enemy.Info.attackCoolDown);
        _canAttack = true;
    }

    private bool PlayerInSight()
    {
        return _sensor.PlayerDistance <= _enemy.Info.attackRange;
    }
}
