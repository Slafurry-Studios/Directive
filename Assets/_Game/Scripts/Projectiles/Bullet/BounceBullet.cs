using UnityEngine;
public class BounceBullet : BaseProjectile
{
    protected override void Move()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}