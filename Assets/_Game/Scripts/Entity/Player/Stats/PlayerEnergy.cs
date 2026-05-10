using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : Energy
{
    [Header("VFX")]
    [SerializeField] private GameObject energyVFX;

    protected override void Start()
    {
        base.Start();

        OnEnergyChanged += HandleEnergyChanged;
        UpdateUI(CurrentEnergy, MaxEnergy);
    }

    private void HandleEnergyChanged(int current, int max)
    {
        UpdateUI(current, max);
    }

    private void UpdateUI(int current, int max)
    {
        if (EnergyHUD.Instance != null)
        {
            EnergyHUD.Instance.UpdateUI(current, max);
        }
    }

    public override void RegainEnergy(int amount)
    {
        base.RegainEnergy(amount);
        Instantiate(energyVFX, transform.position, Quaternion.identity);
    }
}