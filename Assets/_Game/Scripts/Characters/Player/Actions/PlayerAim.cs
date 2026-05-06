using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private int mouseButton = 1;
    
    public bool isAiming;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(mouseButton))
        {
            HandleAiming();
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
    }

    void HandleAiming()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z; 

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos);
        Vector2 lookDirection = (worldPosition - transform.position).normalized;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle + 90f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}