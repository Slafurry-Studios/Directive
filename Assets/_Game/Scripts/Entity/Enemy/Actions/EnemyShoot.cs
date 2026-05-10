using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip shootSound;
    [Range(0, 10f)]
    [SerializeField] private float shootSoundVolume;

    private Enemy _enemy;
    private PatternSpawner _spawner;
    private bool _canAttack = true;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
        _spawner = GetComponent<PatternSpawner>();

        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    public void RequestAttack(Vector2 direction)
    {
        if (_canAttack)
        {
            StartCoroutine(AttackRoutine(direction));
            _enemy.BodyAnimator.Play("shoot");
        }
    }

    private IEnumerator AttackRoutine(Vector2 direction)
    {
        _canAttack = false;

        _spawner.ExecutePattern(_enemy.Info.attack.damage, direction, firePoint.transform);

        if (SfxPlayer.Instance != null) SfxPlayer.Instance.PlayEnemySfx(clip: shootSound, volume: shootSoundVolume, loop: false);

        yield return new WaitForSeconds(_enemy.Info.attack.attackCoolDown);

        _canAttack = true;
    }

    public bool IsReadyToShoot() => _canAttack;
}