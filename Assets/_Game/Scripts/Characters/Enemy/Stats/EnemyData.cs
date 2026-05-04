using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public Sprite sprite;
    public bool armoured;
    public EnemyType type;
    public float moveSpeed;
    [Range(50f, 100f)] public float rotationSpeed = 100f;
    public float shootSpeed;
    public float attackRange;
    public float attackCoolDown;
}

public enum EnemyType
{
    Minion,
    Boss,
    Tower,
}