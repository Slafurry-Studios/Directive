using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] objectToActivate;
    [SerializeField] GameObject[] objectToDeactivate;

    void Awake()
    {
        foreach (var obj in objectToActivate)
        {
            if (obj != null) obj.SetActive(false);
        }

        foreach (var obj in objectToDeactivate)
        {
            if (obj != null) obj.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Opened();
    }

    private void Opened()
    {
        Debug.Log($"[RoomTrigger] {gameObject.name} has been triggered!");

        foreach (var obj in objectToActivate)
        {
            Debug.Log($"[RoomTrigger] {gameObject.name} has activating {obj.name}");
            if (obj != null) obj.SetActive(true);
        }

        foreach (var obj in objectToDeactivate)
        {
            if (obj != null) obj.SetActive(false);
        }
    }
}