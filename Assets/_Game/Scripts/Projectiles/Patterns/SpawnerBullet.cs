using System.Collections;
using UnityEngine;

public class SpawnerBullet : MonoBehaviour
{
    public enum SpawnType
    {
        Straight,
        Circle,
        Spread,
        Burst,
        Spiral,
        Random
    }

    [Header("Bullet")]
    public GameObject bulletPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private SpawnType spawnType;
    [SerializeField] private float fireRate = 1f;

    [Header("Spread Settings")]
    [SerializeField] private int spreadCount = 5;
    [SerializeField] private float spreadAngle = 45f;

    [Header("Circle Settings")]
    [SerializeField] private int circleCount = 12;
    [SerializeField] private float circleRotationSpeed = 50f;
    private float currentCircleAngle = 0f;

    [Header("Burst Settings")]
    [SerializeField] private int burstCount = 5;
    [SerializeField] private float burstDelay = 0.1f;
    [SerializeField] private float burstCooldown = 2f;
    private bool isBursting = false;

    [Header("Spiral Settings")]
    [SerializeField] private float spiralStep = 15f;
    private float currentSpiralAngle = 0f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // Circle rotation (tanpa offset)
        if (spawnType == SpawnType.Circle)
        {
            currentCircleAngle += circleRotationSpeed * Time.deltaTime;
        }

        // Spiral tetap independent
        if (spawnType == SpawnType.Spiral)
        {
            currentSpiralAngle += spiralStep;
        }

        if (timer >= fireRate)
        {
            HandleFire();
            timer = 0f;
        }
    }

    void HandleFire()
    {
        switch (spawnType)
        {
            case SpawnType.Straight:
                FireSingle(transform.right);
                break;

            case SpawnType.Circle:
                FireCircle();
                break;

            case SpawnType.Spread:
                FireSpread();
                break;

            case SpawnType.Burst:
                if (!isBursting)
                    StartCoroutine(FireBurst());
                break;

            case SpawnType.Spiral:
                FireSpiral();
                break;

            case SpawnType.Random:
                FireRandom();
                break;
        }
    }

    void FireSingle(Vector2 direction)
    {
        SpawnBullet(direction);
    }

    void FireCircle()
    {
        float angleStep = 360f / circleCount;

        for (int i = 0; i < circleCount; i++)
        {
            float angle = i * angleStep + currentCircleAngle;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            SpawnBullet(dir);
        }
    }

    void FireSpread()
    {
        float startAngle = -spreadAngle / 2f;
        float step = spreadAngle / (spreadCount - 1);

        for (int i = 0; i < spreadCount; i++)
        {
            float angle = startAngle + step * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * transform.right;
            SpawnBullet(dir);
        }
    }

    IEnumerator FireBurst()
    {
        isBursting = true;

        for (int i = 0; i < burstCount; i++)
        {
            FireSingle(transform.right);
            yield return new WaitForSeconds(burstDelay);
        }

        yield return new WaitForSeconds(burstCooldown);
        isBursting = false;
    }

    void FireSpiral()
    {
        Vector2 dir = Quaternion.Euler(0, 0, currentSpiralAngle) * Vector2.right;
        SpawnBullet(dir);
    }

    void FireRandom()
    {
        float angle = Random.Range(0f, 360f);
        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
        SpawnBullet(dir);
    }

    void SpawnBullet(Vector2 direction)
    {
        if (!bulletPrefab) return;

        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        BaseProjectile projectile = bulletObj.GetComponent<BaseProjectile>();

        if (projectile != null)
        {
            projectile.Setup(direction);
        }
    }
}