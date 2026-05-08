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

    private void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
        EnergyBarFiller();
    }

    private void HandleEnergyChanged(int current, int max)
    {
        UpdateUI(current, max);
    }

    private void UpdateUI(int current, int max)
    {
        Debug.Log($"Player Energy: {current} / {max}");
    }

    private void EnergyBarFiller()
    {
        float energyRatio = (float)CurrentEnergy / MaxEnergy;

        if (energyBar != null)
            energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount, energyRatio, lerpSpeed);

        if (energyPoints.Length > 0)
        {
            for (int i = 0; i < energyPoints.Length; i++)
            {
                energyPoints[i].enabled = DisplayEnergyPoint(currentEnergy, i);
            }
        }
    }

    private bool DisplayEnergyPoint(float _energy, int pointNumber)
    {
        return ((pointNumber * 10) < _energy);
    }
}