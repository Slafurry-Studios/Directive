using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyInfo _enemyInfo;
    [SerializeField] private GameObject _target;
    private EnemySensor _sensor;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _sensor = GetComponent<EnemySensor>();
    }

    public EnemySensor Sensor => _sensor;
    public EnemyInfo Info => _enemyInfo;
    public GameObject Target => _target;
}

