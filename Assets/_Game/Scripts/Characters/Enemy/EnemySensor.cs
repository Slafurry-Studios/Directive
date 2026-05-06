using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemySensor : MonoBehaviour
{
    [SerializeField] private Vector2 playerPos;
    [SerializeField] private float playerDistance;
    private Enemy _enemy;

    void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    void FixedUpdate()
    {
        UpdatePlayerDistance();
    }

    private void UpdatePlayerDistance()
    {
        if (_enemy.Target == null) return;

        float distance = Vector2.Distance(transform.position, _enemy.Target.transform.position);

        playerPos = _enemy.Target.transform.position;
        playerDistance = distance;
    }
    public Vector2 PlayerPos => playerPos;
    public float PlayerDistance => playerDistance;
}