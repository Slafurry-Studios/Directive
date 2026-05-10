using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField]
    [Tooltip("The prefab to be spawned.")]
    private GameObject projectilePrefab;

    [Header("Identity Settings")]
    [SerializeField]
    [Tooltip("Select the layer for the spawned bullet.")]
    private LayerMask projectileLayer;

    [SerializeField]
    [Tooltip("Enemy layer for bullet to detect hit.")]
    private LayerMask enemyLayerForBullet;

    [TagSelector]
    [SerializeField]
    [Tooltip("Select the tag for the spawned projectile.")]
    private string projectileTag = "Untagged";

    public void SpawnProjectile(Transform spawnPoint, Vector2 direction, int damage, int projectileSpeed)
    {
        if (projectilePrefab == null) return;

        GameObject obj = ObjectPool.Instance.Get(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        if (obj == null) return;

        BaseProjectile newProjectile = obj.GetComponent<BaseProjectile>();
        if (newProjectile == null) return;

        newProjectile.gameObject.layer = MaskToLayer(projectileLayer);
        if (!string.IsNullOrEmpty(projectileTag))
            newProjectile.gameObject.tag = projectileTag;

        if (enemyLayerForBullet.value != 0)
            newProjectile.SetEnemyLayer(enemyLayerForBullet);

        newProjectile.Setup(direction, damage, projectileSpeed);
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