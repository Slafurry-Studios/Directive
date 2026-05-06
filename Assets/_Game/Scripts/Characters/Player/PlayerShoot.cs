using UnityEngine;

// ============ PLAYER SHOOTING LOGIC ============
public class PlayerShoot : MonoBehaviour
{
    // ============ DESIGNER CONFIGURATION ============
    [Header("Input Settings")]
    [SerializeField]
    private KeyCode shootKey = KeyCode.Mouse0;
    
    [SerializeField] [Tooltip("The damage dealt by each bullet.")]
    private int damage = 10;

    [Header("References")]
    [SerializeField]
    [Tooltip("The point where the bullet will originate from.")]
    private Transform firePoint;
    private Animator animator;

    // ============ INTERNAL STATE ============
    private ProjectileSpawner spawner;

    // ============ UNITY EVENTS ============
    private void Awake()
    {
        spawner = GetComponent<ProjectileSpawner>();
        animator = GetComponent<Animator>();
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
            animator.SetTrigger("Shoot");
        }
    }

    private void Shoot()
    {
        if (firePoint == null || spawner == null) return;

        Vector2 shootDirection = firePoint.right;

        spawner.SpawnProjectile(firePoint.transform, shootDirection, damage);
    }
}