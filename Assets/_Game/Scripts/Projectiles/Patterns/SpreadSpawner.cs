using UnityEngine;

public class SpreadSpawner : PatternSpawner
{
    [Header("Spread Configuration")]
    [SerializeField] private int spreadCount = 5;
    [SerializeField] private float spreadAngle = 45f;

    public override void ExecutePattern(int damage, Vector2 direction, Transform position)
    {
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - (spreadAngle / 2f);
        float step = (spreadCount > 1) ? spreadAngle / (spreadCount - 1) : 0;

        for (int i = 0; i < spreadCount; i++)
        {
            float angle = startAngle + (step * i);
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            SpawnProjectile(position, dir, damage);
        }
    }
}