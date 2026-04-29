// TargetRegistry.cs
using UnityEngine;
using System.Collections.Generic;

public class TargetRegistry : MonoBehaviour
{
    public static TargetRegistry Instance { get; private set; }

    private readonly List<Transform> targets = new List<Transform>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Register(Transform target)
    {
        if (!targets.Contains(target))
            targets.Add(target);
    }

    public void Unregister(Transform target)
    {
        targets.Remove(target);
    }

    public List<Transform> GetTargets() => targets;
}