using UnityEngine;

// ============ PLAYER SHOOTING LOGIC ============
public class PlayerShoot : MonoBehaviour
{
    // ============ DESIGNER CONFIGURATION ============
    [Header("Input Settings")]
    [SerializeField] [Tooltip("The key used to trigger a shot.")]
    private KeyCode shootKey = KeyCode.Mouse0;

    [Header("References")]
    [SerializeField] [Tooltip("The point where the bullet will originate from.")]
    private Transform firePoint;

    // ============ INTERNAL STATE ============
    private PatternSpawner spawner;

    // ============ UNITY EVENTS ============
    private void Awake()
    {
        spawner = GetComponent<PatternSpawner>();
    }

    private void Update()
    {
        HandleShootInput();
    }

    // ============ LOGIC ============
    private void HandleShootInput()
    {
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (firePoint == null || spawner == null) return;

        Vector2 shootDirection = firePoint.right;
        
        spawner.ExecutePattern();
    }
}