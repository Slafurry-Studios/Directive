using UnityEngine;
public class NormalBullet : BaseProjectile
{
    protected override void Move()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject, lifeTime);
    }
}