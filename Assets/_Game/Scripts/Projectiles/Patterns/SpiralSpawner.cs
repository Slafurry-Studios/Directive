using UnityEngine;

public class SpiralSpawner : PatternSpawner
{
    [Header("Spiral Configuration")]
    [SerializeField] private float spiralStep = 15f;

    private float currentSpiralAngle = 0f;

    public override void ExecutePattern(int damage, Vector2 direction, Transform position)
    {
        Vector2 dir = Quaternion.Euler(0, 0, currentSpiralAngle) * Vector2.right;
        SpawnProjectile(position, dir, damage);
        currentSpiralAngle += spiralStep;
    }
}