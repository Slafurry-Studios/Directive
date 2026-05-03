using UnityEngine;

public class RandomSpawner : PatternSpawner
{
    protected override void ExecutePattern()
    {
        float angle = Random.Range(0f, 360f);
        Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
        SpawnProjectile(transform, dir);
    }
}