using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    private Enemy _enemy;
    private EnemySensor _sensor;
    private bool _isDashing = false;
    private float _dashTimeRemaining = 0f;

    [Header("Dash Settings")]
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashSpeedMultiplier = 2f;

    void Start()
    {
        _enemy = GetComponent<Enemy>();
        _sensor = GetComponent<EnemySensor>();
    }

    void Update()
    {
        if (PlayerInSight() && !_isDashing)
        {
            StartDash();
        }

        if (_isDashing)
        {
            HandleDash();
        }
    }

    private void StartDash()
    {
        _isDashing = true;
        _dashTimeRemaining = dashDuration;
    }

    private void HandleDash()
    {
        if (_dashTimeRemaining > 0)
        {
            Vector2 direction = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(direction * _enemy.Info.moveSpeed * dashSpeedMultiplier * Time.deltaTime);
            _dashTimeRemaining -= Time.deltaTime;
        }
        else
        {
            _isDashing = false;
        }
    }

    private bool PlayerInSight()
    {
        return _sensor.PlayerDistance <= _enemy.Info.attackRange;
    }
}