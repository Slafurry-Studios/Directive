using UnityEngine;

public class RotatingBullet : BaseProjectile
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 360f;

    protected override void Move()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        RaycastHit2D hit = BoxCastForward(enemyLayer);
        if (hit.collider != null)
        {
            HandleHit(hit.collider);
        }
    }
}