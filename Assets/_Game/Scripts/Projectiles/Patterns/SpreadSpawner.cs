using UnityEngine;

public class SpreadSpawner : PatternSpawner
{
    [Header("Spread Configuration")]
    [SerializeField] [Tooltip("Total number of projectiles in one spread.")]
    private int spreadCount = 5;
    [SerializeField] [Tooltip("The total arc angle of the spread.")]
    private float spreadAngle = 45f;

    public override void ExecutePattern()
    {
        float startAngle = -spreadAngle / 2f;
        float step = (spreadCount > 1) ? spreadAngle / (spreadCount - 1) : 0;

        for (int i = 0; i < spreadCount; i++)
        {
            float angle = startAngle + (step * i);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * transform.right;
            SpawnProjectile(transform, dir);
        }
    }
}