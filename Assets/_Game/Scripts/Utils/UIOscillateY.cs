using UnityEngine;

public class UIOscillateY : MonoBehaviour
{
    [SerializeField] private float amplitude = 8f;
    [SerializeField] private float speed = 2f;

    private RectTransform rect;
    private Vector2 startPos;
    private bool isActive;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
    }

    void OnEnable()
    {
        isActive = true;
    }

    void OnDisable()
    {
        isActive = false;
        rect.anchoredPosition = startPos;
    }

    void Update()
    {
        if (!isActive) return;

        float yOffset = Mathf.Sin(Time.unscaledTime * speed) * amplitude;
        rect.anchoredPosition = startPos + Vector2.up * yOffset;
    }
}
