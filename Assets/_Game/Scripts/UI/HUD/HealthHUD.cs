public class HealthHUD : BarHUD
{
    public static HealthHUD Instance;
    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
}