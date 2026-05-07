using UnityEngine;

public class SniperBrain : EnemyBrain
{
    [SerializeField] private float stopDistance = 8f;
    [SerializeField] private float retreatDistance = 4f;

    protected override void ExecuteBehavior()
    {
        float dist = _sensor.PlayerDistance;

        if (dist < retreatDistance)
        {
            Vector2 retreatDir = (transform.position - _enemyInfo.Target.transform.position).normalized;
            _moveController.MoveTo((Vector2)transform.position + retreatDir * 2f);
        }
        else if (dist > stopDistance)
        {
            _moveController.MoveTo(_sensor.PlayerPos);
        }
        else
        {
            _moveController.Stop();
            if (_shootController.IsReadyToShoot())
            {
                _shootController.RequestAttack((_sensor.PlayerPos - (Vector2)transform.position).normalized);
            }
        }
    }
}