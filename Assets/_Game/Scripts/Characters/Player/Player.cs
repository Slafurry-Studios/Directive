using UnityEngine;

public class Player : MonoBehaviour
{
    // ================= COMPONENTS =================
    private PlayerHealth playerHealth;
    private PlayerMove playerMove;
    private PlayerDash playerDash;
    private PlayerEnergy playerEnergy;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerDash = GetComponent<PlayerDash>();
        playerEnergy = GetComponent<PlayerEnergy>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void OnDashStart()
    {
        playerMove.enabled = false;
    }

    public void ResetCondition()
    {
        playerMove.enabled = true;
    }
}