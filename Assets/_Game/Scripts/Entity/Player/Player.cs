using UnityEngine;

public class Player : MonoBehaviour
{
    // ================= COMPONENTS =================
    public PlayerHealth playerHealth { get; private set; }
    public PlayerEnergy playerEnergy { get; private set; }

    public PlayerMove playerMove { get; private set; }
    public PlayerDash playerDash { get; private set; }
    public PlayerAim playerAim { get; private set; }
    public PlayerShoot playerShoot { get; private set; }

    public ProjectileSpawner projectileSpawner { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public Collider2D Collider { get; private set; }

    public Animator bodyAnim { get; private set; }
    public Animator feetAnim { get; private set; }
    public SpriteRenderer[] spriteRenderers { get; private set; }
    void Awake()
    {
        playerEnergy = GetComponentInChildren<PlayerEnergy>();
        playerHealth = GetComponentInChildren<PlayerHealth>();

        playerMove = GetComponentInChildren<PlayerMove>();
        playerDash = GetComponentInChildren<PlayerDash>();
        playerAim = GetComponentInChildren<PlayerAim>();
        playerShoot = GetComponentInChildren<PlayerShoot>();

        projectileSpawner = GetComponentInChildren<ProjectileSpawner>();

        Animator[] animators = GetComponentsInChildren<Animator>();
        bodyAnim = animators[0];
        feetAnim = animators[1];

        rb = GetComponentInChildren<Rigidbody2D>();
        Collider = GetComponentInChildren<Collider2D>();

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void Activate()
    {
        playerAim.enabled = true;
        playerDash.enabled = true;
        playerMove.enabled = true;
        playerShoot.enabled = true;
        playerAim.enabled = true;
    }

    public void Deactivate()
    {
        playerAim.enabled = false;
        playerDash.enabled = false;
        playerMove.enabled = false;
        playerShoot.enabled = false;
        playerAim.enabled = false;
    }


    public void OnDashStart()
    {
        playerMove.enabled = false;
        playerAim.enabled = false;
    }

    public void ResetCondition()
    {
        playerMove.enabled = true;
        playerAim.enabled = true;
    }

    public bool IsAiming()
    {
        return playerAim.isAiming;
    }
}