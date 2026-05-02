using UnityEngine;

// ============ PLAYER SHOOTING LOGIC ============
[RequireComponent(typeof(ProjectileSpawner))]
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
    private ProjectileSpawner spawner;

    // ============ UNITY EVENTS ============
    private void Awake()
    {
        spawner = GetComponent<ProjectileSpawner>();
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
        
        spawner.SpawnProjectile(firePoint, shootDirection);
    }
}