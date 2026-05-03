public class StraightSpawner : PatternSpawner
{
    protected override void ExecutePattern()
    {
        SpawnProjectile(transform, transform.right);
    }
}