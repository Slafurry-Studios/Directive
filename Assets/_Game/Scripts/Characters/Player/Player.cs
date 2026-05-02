using UnityEngine;

public class Player : MonoBehaviour
{
    // ================= COMPONENTS =================
    private PlayerHealth playerHealth;
    private PlayerMove playerMove;
    private PlayerDash playerDash;
    private PlayerEnergy playerEnergy;
    private PlayerAim playerAim;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerDash = GetComponent<PlayerDash>();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAim = GetComponent<PlayerAim>();
    }

    public void OnDashStart()
    {
        playerMove.enabled = false;
        playerAim.enabled = false;
    }

    public void ResetCondition()
    {
        playerMove.enabled = true;
        playerAim.enabled = true;
    }

    public bool IsAiming()
    {
        return playerAim.isAiming;
    }
}