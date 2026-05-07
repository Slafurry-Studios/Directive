using UnityEngine;

public class CircleSpawner : PatternSpawner
{
    [Header("Circle Configuration")]
    [SerializeField] private int circleCount = 12;
    [SerializeField] private float rotationSpeed = 50f;

    private float currentOffsetAngle = 0f;

    public override void ExecutePattern(int damage, Vector2 direction, Transform position)
    {
        float angleStep = 360f / circleCount;

        for (int i = 0; i < circleCount; i++)
        {
            float angle = (i * angleStep) + currentOffsetAngle;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            
            SpawnProjectile(position, dir, damage);
        }
    }
}