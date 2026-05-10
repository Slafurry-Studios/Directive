using System.Collections;
using UnityEngine;

public class BossBrain : EnemyBrain
{

    [Header("Distance Settings")]
    [SerializeField] private float preferredDistance = 6f;
    [SerializeField] private float tolerance = 1.2f;

    [Header("Phase 1 — Normal")]
    [SerializeField] private float p1_burstCount = 3;
    [SerializeField] private float p1_burstDelay = 0.18f;
    [SerializeField] private float p1_dodgeChancePerSec = 0.35f;
    [SerializeField] private float p1_repositionInterval = 2.5f;

    [Header("Phase 2 — Enraged (HP <= 50%)")]
    [SerializeField] private float p2_preferredDistance = 3f;
    [SerializeField] private float p2_burstCount = 5;
    [SerializeField] private float p2_burstDelay = 0.10f;
    [SerializeField] private float p2_dodgeChancePerSec = 0.75f;
    [SerializeField] private float p2_repositionInterval = 1.2f;
    [SerializeField] private float p2_dashShootComboChancePerSec = 0.5f;

    [Header("Strafe")]
    [SerializeField] private float strafeSpeed = 2.5f;
    [SerializeField] private float strafeChangeInterval = 1.4f;


    private EnemyHealth _health;

    private bool _isEnraged = false;
    private bool _isBusy = false;          

    private float _repositionTimer = 0f;
    private float _strafeTimer = 0f;
    private int   _strafeDir = 1;      


    protected override void Awake()
    {
        base.Awake();
        _health = GetComponentInChildren<EnemyHealth>();
    }


    protected override void ExecuteBehavior()
    {
        CheckPhase();

        if (_isBusy) return;    

        if (_isEnraged)
            EnragedBehavior();
        else
            NormalBehavior();
    }


    private void CheckPhase()
    {
        if (_isEnraged) return;

        float healthPercent = _health.CurrentHealth / _health.MaxHealth;
        if (_health != null && healthPercent <= 0.5f)
        {
            _isEnraged = true;
            _repositionTimer = 0f;
            Debug.Log("[BossBrain] ENRAGED — Phase 2 activated!");
        }
    }

    private void NormalBehavior()
    {
        float dist  = _sensor.PlayerDistance;
        float min   = preferredDistance - tolerance;
        float max   = preferredDistance + tolerance;

        if (dist < min || dist > max)
        {
            Vector2 dir       = ((Vector2)transform.position - _sensor.PlayerPos).normalized;
            Vector2 targetPos = _sensor.PlayerPos + dir * preferredDistance;
            _moveController.MoveTo(targetPos);
        }
        else
        {
            Strafe();
        }

        _moveController.RotateTo(_sensor.PlayerPos);

        if (_sensor.IsPlayerInAttackCone && _shootController.IsReadyToShoot())
            StartCoroutine(ShootBurst(p1_burstCount, p1_burstDelay));

        if (_dash.CanDash() && Random.value < p1_dodgeChancePerSec * Time.deltaTime)
            DodgeSideways();

        _repositionTimer -= Time.deltaTime;
        if (_repositionTimer <= 0f)
        {
            _repositionTimer = p1_repositionInterval;
            StartCoroutine(QuickDashReposition());
        }
    }
    private void EnragedBehavior()
    {
        float dist  = _sensor.PlayerDistance;
        float min   = p2_preferredDistance - tolerance;
        float max   = p2_preferredDistance + tolerance;

        if (dist > max)
        {
            Vector2 dir       = ((Vector2)transform.position - _sensor.PlayerPos).normalized;
            Vector2 targetPos = _sensor.PlayerPos + dir * p2_preferredDistance;
            _moveController.MoveTo(targetPos);
        }
        else if (dist < min)
        {
            Vector2 dir       = ((Vector2)transform.position - _sensor.PlayerPos).normalized;
            _moveController.MoveTo((Vector2)transform.position + dir * 1.5f);
        }
        else
        {
            Strafe();
        }

        _moveController.RotateTo(_sensor.PlayerPos);

        if (_dash.CanDash() && Random.value < p2_dashShootComboChancePerSec * Time.deltaTime)
        {
            StartCoroutine(DashShootCombo());
            return;
        }

        if (_sensor.IsPlayerInAttackCone && _shootController.IsReadyToShoot())
            StartCoroutine(ShootBurst(p2_burstCount, p2_burstDelay));

        if (_dash.CanDash() && Random.value < p2_dodgeChancePerSec * Time.deltaTime)
            DodgeSideways();

        _repositionTimer -= Time.deltaTime;
        if (_repositionTimer <= 0f)
        {
            _repositionTimer = p2_repositionInterval;
            StartCoroutine(QuickDashReposition());
        }
    }

    private void Strafe()
    {
        _strafeTimer -= Time.deltaTime;
        if (_strafeTimer <= 0f)
        {
            _strafeDir    = Random.value < 0.5f ? 1 : -1;
            _strafeTimer  = strafeChangeInterval;
        }

        Vector2 toPlayer  = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
        Vector2 side      = Vector2.Perpendicular(toPlayer) * _strafeDir;
        Vector2 strafe    = (Vector2)transform.position + side * strafeSpeed * Time.deltaTime;
        _moveController.MoveTo(strafe);
    }

    private void DodgeSideways()
    {
        Vector2 toPlayer = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
        Vector2 side     = Vector2.Perpendicular(toPlayer);
        if (Random.value < 0.5f) side = -side;
        _dash.RequestDash(side);
    }


    private IEnumerator ShootBurst(float count, float delay)
    {
        _isBusy = true;

        for (int i = 0; i < count; i++)
        {
            if (_health.IsDead) break;

            Vector2 dir = (_sensor.PlayerPos - (Vector2)transform.position).normalized;

            float spread = Random.Range(-5f, 5f);
            dir = Quaternion.Euler(0, 0, spread) * dir;

            _shootController.RequestAttack(dir);
            yield return new WaitForSeconds(delay);
        }

        _isBusy = false;
    }

    private IEnumerator DashShootCombo()
    {
        _isBusy = true;

        Vector2 toPlayer = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
        _dash.RequestDash(toPlayer);

        yield return new WaitForSeconds(0.12f);

        int shots = _isEnraged ? (int)p2_burstCount : (int)p1_burstCount;
        for (int i = 0; i < shots; i++)
        {
            if (_health.IsDead) break;

            Vector2 dir    = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
            float   spread = Random.Range(-8f, 8f);
            dir = Quaternion.Euler(0, 0, spread) * dir;
            _shootController.RequestAttack(dir);

            yield return new WaitForSeconds(_isEnraged ? p2_burstDelay : p1_burstDelay);
        }

        yield return new WaitForSeconds(0.08f);

        if (_dash.CanDash())
            DodgeSideways();

        _isBusy = false;
    }

    private IEnumerator QuickDashReposition()
    {
        _isBusy = true;

        if (_dash.CanDash())
        {
            float   angle     = Random.Range(45f, 135f) * (Random.value < 0.5f ? 1f : -1f);
            Vector2 toPlayer  = (_sensor.PlayerPos - (Vector2)transform.position).normalized;
            Vector2 dashDir   = Quaternion.Euler(0, 0, angle) * toPlayer;
            _dash.RequestDash(dashDir);
        }

        yield return new WaitForSeconds(0.25f);

        _isBusy = false;
    }
}