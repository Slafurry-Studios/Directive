public class StraightSpawner : PatternSpawner
{
    public override void ExecutePattern()
    {
        SpawnProjectile(transform, transform.right);
    }
}