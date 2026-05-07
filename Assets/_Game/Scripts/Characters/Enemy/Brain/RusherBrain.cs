using UnityEngine;
using System.Collections;

public class RusherBrain : EnemyBrain
{
    [Header("Zigzag Settings")]
    [SerializeField] private float zigzagInterval = 0.8f;
    [SerializeField] private float zigzagAngle = 45f;

    [Header("Attack Settings")]
    [SerializeField] private float attackPreDelay = 0.5f;
    [SerializeField] private int dashesBeforeAttack = 3;

    private int _dashCount = 0;
    private float _nextDashTime;
    private bool _isActionLocked;

    protected override void ExecuteBehavior()
    {
        if (_dash.IsDashing || _isActionLocked) return;

        if (_dashCount < dashesBeforeAttack)
        {
            HandleZigzagDash();
        }
        else
        {
            StartCoroutine(LungeAttackSequence());
        }
    }

    private void HandleZigzagDash()
    {
        if (Time.time >= _nextDashTime && _dash.CanDash())
        {
            Vector2 dirToPlayer = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
            
            float sideAngle = (_dashCount % 2 == 0) ? zigzagAngle : -zigzagAngle;
            Vector2 zigzagDir = Quaternion.Euler(0, 0, sideAngle) * dirToPlayer;

            _dash.RequestDash(zigzagDir);
            
            _dashCount++;
            _nextDashTime = Time.time + zigzagInterval;
        }
        else
        {
            _moveController.MoveTo(_sensor.PlayerPos);
        }
    }

    private IEnumerator LungeAttackSequence()
    {
        _isActionLocked = true;
        _moveController.Stop();

        Debug.Log("Rusher is Charging Lunge!");
        
        yield return new WaitForSeconds(attackPreDelay);

        Vector2 finalDir = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
        _dash.RequestDash(finalDir);

        yield return new WaitUntil(() => !_dash.IsDashing);
        _dashCount = 0;
        _nextDashTime = Time.time + zigzagInterval;
        _isActionLocked = false;
    }
}