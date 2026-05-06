using UnityEngine;
using System.Collections;

public abstract class PatternSpawner : ProjectileSpawner
{
    // [Header("Pattern Timing")]
    // [SerializeField] [Tooltip("How many seconds between firing intervals.")]
    // protected float fireRate = 1f;

    // protected float timer = 0f;

    protected virtual void Update()
    {
        // THE ACTUAL FIRING IS CONTROLLED BY THE ENEMY SCRIPT, SO THIS IS COMMENTED OUT TO AVOID CONFLICTS
        // timer += Time.deltaTime;
        // if (timer >= fireRate)
        // {
        //     ExecutePattern();
        //     timer = 0f;
        // }
    }

    public abstract void ExecutePattern(int damage);
}