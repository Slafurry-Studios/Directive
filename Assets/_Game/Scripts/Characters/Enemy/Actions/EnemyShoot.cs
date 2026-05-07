using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;

    private Enemy _data;
    private PatternSpawner _spawner;
    private bool _canAttack = true;

    private void Awake()
    {
        _data = GetComponentInParent<Enemy>();
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

        _spawner.ExecutePattern(_data.Info.attack.damage, direction, firePoint.transform);

        yield return new WaitForSeconds(_data.Info.attack.attackCoolDown);
        
        _canAttack = true;
    }

    public bool IsReadyToShoot() => _canAttack;
}