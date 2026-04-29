using UnityEngine;

public class PlayerHealth : Health
{
    protected override void Start()
    {
        base.Start();
        OnHealthChanged += UpdateUI;
        OnDeath += HandlePlayerDeath;
    }
    private void UpdateUI(int current, int max)
    {
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player died! Show Game Over.");
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
    }

}