using UnityEngine;

public class RandomSpawner : PatternSpawner
{
    public override void ExecutePattern(int damage, Vector2 direction, Transform position)
    {
        float angle = Random.Range(0f, 360f);
        Vector2 randomDir = Quaternion.Euler(0, 0, angle) * Vector2.right;
        
        SpawnProjectile(position, randomDir, damage);
    }
}