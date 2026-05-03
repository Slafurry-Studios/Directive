using UnityEngine;

public class SpawnerBulletFollow : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bulletPrefab;

    [Header("Fire Settings")]
    [SerializeField] private float fireRate = 1f;

    [Header("Target Settings")]
    [SerializeField] private string playerTag = "Player";

    [Header("Offset Settings")]
    [SerializeField] private float offsetAngle = 0f; // derajat deviasi

    private Transform target;
    private float timer = 0f;

    void Start()
    {
        FindTarget();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (target == null)
        {
            FindTarget();
            return;
        }

        if (timer >= fireRate)
        {
            FireAtTarget();
            timer = 0f;
        }
    }

    void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            target = player.transform;
        }
    }

    void FireAtTarget()
    {
        if (!bulletPrefab || target == null) return;

        // arah utama ke player
        Vector2 direction = (target.position - transform.position).normalized;

        // tambahkan offset sudut
        float randomOffset = Random.Range(-offsetAngle, offsetAngle);
        direction = Quaternion.Euler(0, 0, randomOffset) * direction;

        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        BaseProjectile projectile = bulletObj.GetComponent<BaseProjectile>();

        if (projectile != null)
        {
            projectile.Setup(direction);
        }
    }
}