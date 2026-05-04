using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    public void Shoot()
    {
        if (_enemy.Sensor.PlayerDistance <= _enemy.Info.attackRange)
        {
            Vector3 direction = (_enemy.Sensor.PlayerPos - (Vector2)transform.position).normalized;
            GameObject bullet = Instance.GetProjectile("EnemyBullet");
            bullet.transform.position = transform.position;
            bullet.GetComponent<EnemyBullet>().SetDirection(direction);
        }
    }
}