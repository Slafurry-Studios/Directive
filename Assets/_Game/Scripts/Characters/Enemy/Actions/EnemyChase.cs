using UnityEngine;

[RequireComponent(typeof(EnemySensor))]
[RequireComponent(typeof(Enemy))]
public class EnemyChase : MonoBehaviour
{
    // ============ VARIABLES ============

    [Header("Designer Overrides")]
    [SerializeField] 
    [Tooltip("If enabled, uses the custom speed below. If disabled, uses the speed from the Enemy Info Scriptable Object.")]
    private bool overrideInfoSpeed = false;

    [SerializeField] 
    [Tooltip("The speed used only if 'Override Info Speed' is checked.")]
    private float customMoveSpeed = 5f;

    [Header("Visual Settings")]
    [SerializeField] 
    [Tooltip("Offset for the rotation. Adjust this if the sprite is not facing the player correctly (e.g., 90, -90, 180).")]
    private float angleOffset = 0f;

    private Enemy _enemy;
    private EnemySensor _sensor;

    public float ActiveMoveSpeed { get; private set; }

    // ============ UNITY EVENTS ============

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _sensor = GetComponent<EnemySensor>();

        ActiveMoveSpeed = overrideInfoSpeed ? customMoveSpeed : _enemy.Info.moveSpeed;
    }

    private void FixedUpdate()
    {
        if (_sensor != null && _enemy != null)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    // ============ LOGIC ============

    private void HandleMovement()
    {
        if (_sensor.PlayerDistance > _enemy.Info.attackRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _sensor.PlayerPos,
                ActiveMoveSpeed * Time.fixedDeltaTime
            );
        }
    }

    private void HandleRotation()
    {
        Vector2 direction = _sensor.PlayerPos - (Vector2)transform.position;

        if (direction != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
            float currentAngle = transform.eulerAngles.z;
            
            float smoothedAngle = Mathf.MoveTowardsAngle(
                currentAngle, 
                targetAngle, 
                _enemy.Info.rotationSpeed * Time.fixedDeltaTime
            );

            transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
        }
    }
}