using UnityEngine;
public class BounceBullet : BaseProjectile
{
    protected override void Move()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}