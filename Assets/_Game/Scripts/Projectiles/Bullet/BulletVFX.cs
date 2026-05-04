using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TrailRenderer))]
public class BulletVFX : BaseProjectile
{
    [Header("VFX")]
    public ParticleSystem bulletHitEffect;

    private Rigidbody2D rb;
    private TrailRenderer trail;

    private bool isDestroying = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();

        // Setup Rigidbody agar tetap trigger tanpa physics penuh
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Pastikan collider jadi trigger
        GetComponent<Collider2D>().isTrigger = true;
    }

    protected override void Move()
    {
        if (isDestroying) return;

        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroying) return;

        Debug.Log("Hit: " + collision.name);

        isDestroying = true;

        PlayHitEffect();
        StartCoroutine(DestroyWithTrail());
    }

    IEnumerator DestroyWithTrail()
    {
        // Stop movement
        moveSpeed = 0f;

        // Matikan trail
        if (trail != null)
        {
            trail.emitting = false;
        }

        // Tunggu trail selesai
        float waitTime = trail != null ? trail.time : 0f;
        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
    }

    void PlayHitEffect()
    {
        if (bulletHitEffect != null)
        {
            ParticleSystem effect = Instantiate(
                bulletHitEffect,
                transform.position,
                Quaternion.identity
            );

            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }
    }
}