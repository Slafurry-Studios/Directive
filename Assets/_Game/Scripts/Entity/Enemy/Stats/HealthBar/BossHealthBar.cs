using UnityEngine;

public class BossHealthBar : EnemyHealthBar
{
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    void LateUpdate()
    {
    }
}