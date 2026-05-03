using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyInfo _enemyInfo;
    [SerializeField] private GameObject _target;
    private ProjectileSpawner _spawner;
    private float _projectileSpawnTime;
    private float _projectileSpawnCount;

    private int spawnBulletCount;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _enemyInfo.sprite;
        if (_target == null)
            _target = GameObject.FindGameObjectWithTag("Player");
        _spawner = GetComponent<ProjectileSpawner>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }

    void ChasePlayer()
    {
        if(_enemyInfo.type == EnemyType.Tower) return;
        
        RotateTowardsPlayer();
        
        //Movement Happens here : player stops on reaching the attack range
        float distance = Vector3.Distance(transform.position, _target.transform.position);
        
        if (distance > _enemyInfo.attackRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _target.transform.position,
                _enemyInfo.moveSpeed * Time.deltaTime
            );
        }

        
        if (distance <= _enemyInfo.attackRange)
        {
            var directionVector = (_target.transform.position - this.transform.position).normalized;
            AttackPlayer(directionVector);
            _projectileSpawnTime += Time.deltaTime;
        }
    }
    
    void RotateTowardsPlayer()
    {
        //Rotation Happens here
        Vector3 direction = _target.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle -= 90f;
        
        float currentAngle = transform.eulerAngles.z;
        float smoothedAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, _enemyInfo.rotationSpeed);
        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
    }

    void AttackPlayer(Vector3 direction)
    {
        if (_projectileSpawnCount < _enemyInfo.shootSpeed)
        {
            _spawner.SpawnProjectile(this.transform, direction);
            _projectileSpawnCount++;
        }
        if (_projectileSpawnTime >= _enemyInfo.attackCoolDown)
        {
            _projectileSpawnCount = 0;
            _projectileSpawnTime = 0;
        }
    }
}

public enum EnemyType
{
    Minion,
    Boss,
    Tower,
}

[CreateAssetMenu(menuName = "Enemy/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public Sprite sprite;
    public bool armoured;
    public EnemyType type;
    public float moveSpeed;
    [Range(50f, 100f)] public float rotationSpeed = 100f;
    public float shootSpeed;
    public float attackRange;
    public float attackCoolDown;
}
