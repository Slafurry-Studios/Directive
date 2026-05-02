using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] [Tooltip("The prefab to be spawned. Must have a BaseProjectile script.")]
    private BaseProjectile projectilePrefab;

    public void SpawnProjectile(Transform spawnPoint, Vector2 direction)
    {
        if (projectilePrefab == null) return;

        BaseProjectile newProjectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        newProjectile.Setup(direction);
    }
}