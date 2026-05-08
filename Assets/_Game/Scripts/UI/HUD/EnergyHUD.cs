public class EnergyHUD : BarHUD
{
    public static EnergyHUD Instance;
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
}