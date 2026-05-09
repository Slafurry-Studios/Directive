using UnityEngine;

public abstract class PatternSpawner : ProjectileSpawner
{
    public abstract void ExecutePattern(int damage, Vector2 direction, Transform position);
}