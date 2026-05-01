using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }


}