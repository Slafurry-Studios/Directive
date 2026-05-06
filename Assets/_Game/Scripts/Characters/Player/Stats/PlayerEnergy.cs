using UnityEngine;

public class PlayerEnergy : Energy
{
    protected override void Start()
    {
        base.Start();

        OnEnergyChanged += HandleEnergyChanged;
    }

    private void HandleEnergyChanged(int current, int max)
    {
    }
}