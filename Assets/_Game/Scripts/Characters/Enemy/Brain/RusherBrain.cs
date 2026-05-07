using UnityEngine;
using System.Collections;

public class RusherBrain : EnemyBrain
{
    [Header("Zigzag Settings")]
    [SerializeField] private float zigzagInterval = 0.4f;
    [SerializeField] private float zigzagAngle = 50f;

    [Header("Attack Settings")]
    [SerializeField] private float attackPreDelay = 0.5f;
    [SerializeField] private int dashesBeforeAttack = 2;

    [Header("Dash Indicator")]
    [SerializeField] private LineRenderer dashIndicator;
    [SerializeField] private float indicatorWidth = 0.6f;
    [SerializeField] private float indicatorLength = 5f;

    private int _dashCount = 0;
    private float _nextDashTime;
    private bool _isActionLocked;

    protected override void ExecuteBehavior()
    {
        // Always rotate toward player regardless of state
        _moveController.RotateTo(_sensor.PlayerPos);

        if (_dash.IsDashing || _isActionLocked) return;

        if (_dashCount < dashesBeforeAttack)
            HandleZigzagDash();
        else
            StartCoroutine(LungeAttackSequence());
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

        Vector2 finalDir = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
        ShowIndicator(finalDir, true);

        if (_shootController.IsReadyToShoot())
            _shootController.RequestAttack(finalDir);

        yield return new WaitForSeconds(attackPreDelay);

        ShowIndicator(finalDir, false);
        _dash.RequestDash(finalDir);

        yield return new WaitUntil(() => !_dash.IsDashing);

        _dashCount = 0;
        _nextDashTime = Time.time + zigzagInterval;
        _isActionLocked = false;
    }

    private void ShowIndicator(Vector2 direction, bool visible)
    {
        if (dashIndicator == null) return;

        dashIndicator.enabled = visible;
        if (!visible) return;

        dashIndicator.startWidth = indicatorWidth;
        dashIndicator.endWidth = indicatorWidth;

        dashIndicator.positionCount = 2;
        dashIndicator.SetPosition(0, transform.position);
        dashIndicator.SetPosition(1, transform.position + (Vector3)(direction * indicatorLength));
    }
}