using UnityEngine;
using System.Collections;

public class SniperBrain : EnemyBrain
{
    [Header("Range Settings")]
    [SerializeField] private float stopDistance = 8f;
    [SerializeField] private float retreatDistance = 4f;

    [Header("Charge Settings")]
    [SerializeField] private float chargeTime = 1.5f;

    [Header("Indicator")]
    [SerializeField] private LineRenderer aimIndicator;
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private float gapLength = 0.2f;

    private bool _isCharging = false;
    private bool _isFiring = false;
    private float _chargeTimer = 0f;

    protected override void ExecuteBehavior()
    {
        if (_isFiring) return;

        float dist = _sensor.PlayerDistance;

        if (dist < retreatDistance)
        {
            CancelCharge();
            Vector2 retreatDir = ((Vector2)transform.position - _sensor.PlayerPos).normalized;
            _moveController.MoveTo((Vector2)transform.position + retreatDir * 2f);
        }
        else if (dist > stopDistance)
        {
            CancelCharge();
            _moveController.MoveTo(_sensor.PlayerPos);
        }
        else
        {
            _moveController.Stop();
            _moveController.RotateTo(_sensor.PlayerPos);

            if (!_isCharging)
            {
                _isCharging = true;
                _chargeTimer = 0f;
            }

            _chargeTimer += Time.deltaTime;
            UpdateIndicator(true);

            if (_chargeTimer >= chargeTime)
                StartCoroutine(FireRoutine());
        }
    }

    private IEnumerator FireRoutine()
    {
        _isFiring = true;
        _isCharging = false;
        _chargeTimer = 0f;
        UpdateIndicator(false);

        Vector2 dir = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
        _shootController.RequestAttack(dir);

        yield return new WaitUntil(() => _shootController.IsReadyToShoot());
        _isFiring = false;
    }

    private void CancelCharge()
    {
        _isCharging = false;
        _chargeTimer = 0f;
        UpdateIndicator(false);
    }

    private void UpdateIndicator(bool visible)
    {
        if (aimIndicator == null) return;

        aimIndicator.enabled = visible;
        if (!visible) return;

        float progress = Mathf.Clamp01(_chargeTimer / chargeTime);
        Vector2 origin = transform.position;
        Vector2 dir = (_sensor.PlayerPos - origin).normalized;
        float maxLen = Vector2.Distance(transform.position, _sensor.PlayerPos) * progress;

        var positions = new System.Collections.Generic.List<Vector3>();
        float drawn = 0f;
        bool isDash = true;

        while (drawn < maxLen)
        {
            float segLen = isDash ? dashLength : gapLength;
            float segEnd = Mathf.Min(drawn + segLen, maxLen);

            if (isDash)
            {
                positions.Add(origin + dir * drawn);
                positions.Add(origin + dir * segEnd);
            }

            drawn = segEnd;
            isDash = !isDash;
        }

        aimIndicator.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++)
            aimIndicator.SetPosition(i, positions[i]);
    }
}