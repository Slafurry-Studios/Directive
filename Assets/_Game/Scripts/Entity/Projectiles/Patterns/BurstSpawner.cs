using UnityEngine;
using System.Collections;

public class BurstSpawner : PatternSpawner
{
    [Header("Burst Configuration")]
    [SerializeField] private int burstCount = 5;
    [SerializeField] private float burstDelay = 0.1f;

    private bool isBursting = false;

    public override void ExecutePattern(int damage, Vector2 direction, Transform position, int speed)
    {
        if (!isBursting)
        {
            StartCoroutine(FireBurstSequence(damage, direction, position, speed));
        }
    }

    private IEnumerator FireBurstSequence(int damage, Vector2 direction, Transform position, int speed)
    {
        isBursting = true;
        for (int i = 0; i < burstCount; i++)
        {
            SpawnProjectile(position, direction, damage, speed);
            yield return new WaitForSeconds(burstDelay);
        }
        isBursting = false;
    }
}