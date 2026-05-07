using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemySensor : MonoBehaviour
{
    [Header("Detection FOV (For Shooting)")]
    [SerializeField] private float offset = 0f;
    [SerializeField] private float detectionAngle = 60f;
    [SerializeField] private float attackRange = 10f;

    private Enemy _enemy;
    private Vector2 _playerPos;
    private float _playerDistance;
    private bool _isPlayerInAttackCone;

    public Vector2 PlayerPos => _playerPos;
    public float PlayerDistance => _playerDistance;
    public bool IsPlayerInAttackCone => _isPlayerInAttackCone;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {
        if (_enemy.Target == null) return;

        UpdateBasicPlayerData();
        UpdateAttackConeStatus();
    }

    private void UpdateBasicPlayerData()
    {
        _playerPos = _enemy.Target.transform.position;
        _playerDistance = Vector2.Distance(transform.position, _playerPos);
    }

    private void UpdateAttackConeStatus()
    {
        if (_playerDistance > attackRange)
        {
            _isPlayerInAttackCone = false;
            return;
        }

        Vector2 forwardDirection = GetForwardDirection();
        Vector2 directionToPlayer = (_playerPos - (Vector2)transform.position).normalized;
        
        float angle = Vector2.Angle(forwardDirection, directionToPlayer);
        
        _isPlayerInAttackCone = angle <= detectionAngle / 2f;
    }

    private Vector2 GetForwardDirection()
    {
        float totalAngle = (transform.eulerAngles.z + offset) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(totalAngle), Mathf.Sin(totalAngle));
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 forward = GetForwardDirection();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Vector3 leftBoundary = Quaternion.AngleAxis(-detectionAngle / 2f, Vector3.forward) * forward;
        Vector3 rightBoundary = Quaternion.AngleAxis(detectionAngle / 2f, Vector3.forward) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftBoundary * attackRange);
        Gizmos.DrawRay(transform.position, rightBoundary * attackRange);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, forward * (attackRange * 0.5f));
    }
}