using UnityEngine;

public class NormalBullet : BaseProjectile
{

    public override void Setup(Vector2 launchDirection, int damage, int speed)
    {
        base.Setup(launchDirection, damage, speed);
        StartShrinking();
    }

    protected override void Move()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        RaycastHit2D hit = BoxCastForward(enemyLayer);
        if (hit.collider != null)
        {
            HandleHit(hit.collider);
        }
    }
}