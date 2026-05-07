using UnityEngine;

public class StraightSpawner : PatternSpawner
{
    public override void ExecutePattern(int damage, Vector2 direction, Transform position)
    {
        SpawnProjectile(position, direction, damage);
    }
}