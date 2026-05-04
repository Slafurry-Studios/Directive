using UnityEngine;
using System;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 100;
    protected int currentHealth;
    private bool isDead = false;

    // ================= EVENTS =================
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    // ================= UNITY LIFECYCLE =================
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        NotifyHealthChanged();
    }

    // ================= PUBLIC API =================
    public virtual void TakeDamage(int amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        NotifyHealthChanged();

        if (currentHealth == 0)
            HandleDeath();
    }

    public virtual void Heal(int amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        NotifyHealthChanged();
    }

    // ================= PROTECTED HOOKS =================

    /// <summary>
/// Override this for additional logic on death (animations, item drops, etc) 
/// Always call base.OnDeath() if you want the event to continue
    /// </summary>
    protected virtual void Death()
    {
        // Override in subclass
    }

    // ================= PRIVATE =================
    private void HandleDeath()
    {
        if (isDead) return;
        isDead = true;

        OnDeath?.Invoke();
        Death(); // hook for subclass
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // ================= READ ONLY =================
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;
}