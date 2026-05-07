using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyInfo _enemyInfo;
    [SerializeField] private GameObject _target;
    private EnemySensor _sensor;

    // ============ ACTIONS ==============
    private EnemyDash _dash;
    private EnemyMovement _move;
    private EnemyShoot _shoot;


    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _sensor = GetComponent<EnemySensor>();
        _dash = GetComponent<EnemyDash>();
        _move = GetComponent<EnemyMovement>();
        _shoot = GetComponent<EnemyShoot>();

        Invoke("Dash", 2);
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
    public EnemyInfo Info => _enemyInfo;
    public GameObject Target => _target;
}

