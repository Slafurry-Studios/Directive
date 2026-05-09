using UnityEngine;

public class OscillateY : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.25f;
    [SerializeField] private float speed = 2f;

    private Vector3 startPos;
    private bool isActive;

    void Awake()
    {
        startPos = transform.localPosition;
    }

    void OnEnable()
    {
        isActive = true;
    }

    void OnDisable()
    {
        isActive = false;
        transform.localPosition = startPos;
    }

    void Update()
    {
        if (!isActive) return;

        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;

        transform.localPosition =
            startPos + Vector3.up * yOffset;
    }
}