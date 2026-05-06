using UnityEngine;

public class PlayerEnergy : Energy
{
    protected override void Start()
    {
        base.Start();

        OnEnergyChanged += UpdateUI;
    }

    private void UpdateUI(int current, int max)
    {
    }
}