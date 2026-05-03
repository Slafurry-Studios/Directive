using UnityEngine;
using System.Collections;

public class BurstSpawner : PatternSpawner
{
    [Header("Burst Configuration")]
    [SerializeField] [Tooltip("Number of bullets in a single burst.")]
    private int burstCount = 5;
    [SerializeField] [Tooltip("Time between shots within the burst.")]
    private float burstDelay = 0.1f;

    private bool isBursting = false;

    protected override void ExecutePattern()
    {
        if (!isBursting)
        {
            StartCoroutine(FireBurstSequence());
        }
    }

    private IEnumerator FireBurstSequence()
    {
        isBursting = true;
        for (int i = 0; i < burstCount; i++)
        {
            SpawnProjectile(transform, transform.right);
            yield return new WaitForSeconds(burstDelay);
        }
        isBursting = false;
    }
}