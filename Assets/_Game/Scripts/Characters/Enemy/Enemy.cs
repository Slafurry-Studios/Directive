using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyInfo;
    [SerializeField] private GameObject _target;
    private EnemySensor _sensor;

    // ============ ACTIONS ==============
    private EnemyMovement _move;
    private EnemyShoot _shoot;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _sensor = GetComponentInChildren<EnemySensor>();
        _move = GetComponentInChildren<EnemyMovement>();
        _shoot = GetComponentInChildren<EnemyShoot>();
    }

    public void OnDashStart()
    {
        _move.enabled = false;
        _shoot.enabled = false;
    }

    public void ResetCondition()
    {
        _move.enabled = true;
        _shoot.enabled = true;
    }


    public EnemySensor Sensor => _sensor;
    public EnemyData Info => _enemyInfo;
    public GameObject Target => _target;
}

