using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyInfo;
    private GameObject _target;
    private EnemyCollection _collection;
    private EnemySensor _sensor;

    // ============ ACTIONS ==============
    private EnemyDash _dash;
    private EnemyMovement _move;
    private EnemyShoot _shoot;

    // ============ ANIMATOR ==============
    private Animator[] animators;
    public Animator BodyAnimator { get; private set; }
    public Animator FeetAnimator { get; private set; }


    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _sensor = GetComponentInChildren<EnemySensor>();

        _move = GetComponentInChildren<EnemyMovement>();
        _shoot = GetComponentInChildren<EnemyShoot>();
        _dash = GetComponentInChildren<EnemyDash>();

        _collection = GetComponentInParent<EnemyCollection>();
        animators = GetComponentsInChildren<Animator>();

        BodyAnimator = animators[0];
        FeetAnimator = animators[1];

        _collection.AddEnemy(this);
        EnemyIndicatorManager.Instance.RegisterEnemy(transform);
    }

    public void OnDashStart()
    {
        _move.enabled = false;
        _shoot.enabled = false;
    }

    public void DeactivateAll()
    {
        _move.enabled = false;
        _dash.enabled = false;
        _shoot.enabled = false;
    }

    public void ResetCondition()
    {
        _move.enabled = true;
        _shoot.enabled = true;
    }


    public EnemySensor Sensor => _sensor;
    public EnemyData Info => _enemyInfo;
    public EnemyCollection Collection => _collection;
    public GameObject Target => _target;
}

