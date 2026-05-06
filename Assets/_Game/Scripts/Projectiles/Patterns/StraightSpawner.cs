public class StraightSpawner : PatternSpawner
{
    public override void ExecutePattern(int damage)
    {
        SpawnProjectile(transform, transform.right, damage);
    }
}