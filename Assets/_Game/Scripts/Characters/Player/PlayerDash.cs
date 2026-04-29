using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerEnergy playerEnergy;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        playerEnergy = GetComponentInParent<PlayerEnergy>();
    }
}