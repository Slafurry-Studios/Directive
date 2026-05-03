using UnityEngine;
public class CircleSpawner : PatternSpawner
{
    [Header("Circle Configuration")]
    [SerializeField] [Tooltip("Number of projectiles to form the circle.")]
    private int circleCount = 12;
    [SerializeField] [Tooltip("How fast the circle pattern rotates over time.")]
    private float rotationSpeed = 50f;

    private float currentOffsetAngle = 0f;

    protected override void Update()
    {
        base.Update();
        currentOffsetAngle += rotationSpeed * Time.deltaTime;
    }

    protected override void ExecutePattern()
    {
        float angleStep = 360f / circleCount;

        for (int i = 0; i < circleCount; i++)
        {
            float angle = (i * angleStep) + currentOffsetAngle;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            SpawnProjectile(transform, dir);
        }
    }
}