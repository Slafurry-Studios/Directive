using UnityEngine;
public class NormalBullet : BaseProjectile
{
    [SerializeField] private int damageAmount = 10;

    protected override void Move()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
        // 1. Cari komponen EnemyHealth pada objek yang ditabrak
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

        // 2. Jika objek yang ditabrak adalah musuh (memiliki EnemyHealth)[cite: 5]
        if (enemy != null)
        {
            // 3. Berikan damage ke musuh tersebut[cite: 5]
            enemy.TakeDamage(damageAmount);
            
            // Log untuk memastikan damage terkirim di Console
            Debug.Log("Player hit Enemy! Damage: " + damageAmount);
        }

        // 4. Hancurkan peluru setelah tabrakan (baik kena musuh atau tembok)
        Destroy(gameObject);
    }
}