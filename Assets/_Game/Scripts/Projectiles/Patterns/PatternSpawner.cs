using UnityEngine;
using System.Collections;

// ============ BASE PATTERN LOGIC ============
public abstract class PatternSpawner : ProjectileSpawner
{
    [Header("Pattern Timing")]
    [SerializeField] [Tooltip("How many seconds between firing intervals.")]
    protected float fireRate = 1f;

    protected float timer = 0f;

    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            ExecutePattern();
            timer = 0f;
        }
    }

    protected abstract void ExecutePattern();
}