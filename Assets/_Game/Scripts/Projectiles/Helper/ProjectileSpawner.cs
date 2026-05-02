using UnityEngine;

// ============ SPAWNING SYSTEM ============
public class ProjectileSpawner : MonoBehaviour
{
    // ============ DESIGNER CONFIGURATION ============
    [Header("Projectile Settings")]
    [SerializeField] [Tooltip("The prefab to be spawned.")]
    private BaseProjectile projectilePrefab;

    [Header("Identity Settings")]
    [SerializeField] [Tooltip("Select the layer for the spawned bullet.")]
    private LayerMask projectileLayer;
    [TagSelector]
    [SerializeField] [Tooltip("Select the tag for the spawned projectile.")]
    private string projectileTag = "Untagged";

    // ============ LOGIC ============
    public void SpawnProjectile(Transform spawnPoint, Vector2 direction)
    {
        if (projectilePrefab == null) return;

        BaseProjectile newProjectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        
        // Set Layer
        newProjectile.gameObject.layer = MaskToLayer(projectileLayer);

        // Set Tag
        if (!string.IsNullOrEmpty(projectileTag))
        {
            newProjectile.gameObject.tag = projectileTag;
        }

        newProjectile.Setup(direction);
    }

    private int MaskToLayer(LayerMask mask)
    {
        int bitmask = mask.value;
        if (bitmask == 0) return 0;
        int result = 0;
        while (bitmask > 1)
        {
            bitmask >>= 1;
            result++;
        }
        return result;
    }
}