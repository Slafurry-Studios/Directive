using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public StatsSettings stats;
    public MovementSettings movement;
    public AttackSettings attack;
    public DashSettings dash;
}

[System.Serializable]
public struct StatsSettings
{
    public float maxHealth;
    public int contactDamage;
}

[System.Serializable]
public struct MovementSettings
{
    public float moveSpeed;
    [Range(100f, 1000f)] public float rotationSpeed;
}

[System.Serializable]
public struct AttackSettings
{
    public int damage;
    public int projectileDamage;
    public float attackRange;
    public float attackCoolDown;
    public float projectileSpeed;
}

[System.Serializable]
public struct DashSettings
{
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
}
