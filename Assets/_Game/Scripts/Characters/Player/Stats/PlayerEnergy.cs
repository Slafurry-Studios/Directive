using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : Energy
{
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
}