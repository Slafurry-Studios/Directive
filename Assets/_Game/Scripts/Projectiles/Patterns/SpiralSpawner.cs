using UnityEngine;

public class SpiralSpawner : PatternSpawner
{
    [Header("Spiral Configuration")]
    [SerializeField] [Tooltip("Degrees to rotate the spawn point after every shot.")]
    private float spiralStep = 15f;

    private float currentSpiralAngle = 0f;

    public override void ExecutePattern()
    {
        Vector2 dir = Quaternion.Euler(0, 0, currentSpiralAngle) * Vector2.right;
        SpawnProjectile(transform, dir);
        currentSpiralAngle += spiralStep;
    }
}