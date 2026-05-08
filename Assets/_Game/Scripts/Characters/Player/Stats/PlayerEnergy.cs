using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : Energy
{
    [Header("UI Settings")]
    [SerializeField] public Image energyBar;
    [SerializeField] public Image[] energyPoints;

    private float lerpSpeed;

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