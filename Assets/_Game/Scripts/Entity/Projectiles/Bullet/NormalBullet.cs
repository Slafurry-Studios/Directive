using UnityEngine;

public class NormalBullet : BaseProjectile
{
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