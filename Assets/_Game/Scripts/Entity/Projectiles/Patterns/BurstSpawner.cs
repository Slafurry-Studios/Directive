using UnityEngine;
using System.Collections;

public class BurstSpawner : PatternSpawner
{
    [Header("Burst Configuration")]
    [SerializeField] private int burstCount = 5;
    [SerializeField] private float burstDelay = 0.1f;

    private bool isBursting = false;

    public override void ExecutePattern(int damage, Vector2 direction, Transform position)
    {
        if (!isBursting)
        {
            StartCoroutine(FireBurstSequence(damage, direction, position));
        }
    }

    private IEnumerator FireBurstSequence(int damage, Vector2 direction, Transform position)
    {
        isBursting = true;
        for (int i = 0; i < burstCount; i++)
        {
            SpawnProjectile(position, direction, damage);
            yield return new WaitForSeconds(burstDelay);
        }
        isBursting = false;
    }
}