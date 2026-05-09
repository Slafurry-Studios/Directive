using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    private Transform _enemy;
    private RectTransform _rect;
    private RectTransform _canvasRect;
    private Camera _cam;
    private float _edgePadding = 50f;
    [SerializeField] private float hideDistance = 5f;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _cam = Camera.main;
        _canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public void Init(Transform enemy)
    {
        _enemy = enemy;
    }

    public void UpdateIndicator()
    {
        float distance = Vector3.Distance(_cam.transform.position, _enemy.position);

        Vector2 screenPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect,
            _cam.WorldToScreenPoint(_enemy.position),
            _cam,
            out screenPoint
        );

        float screenW = _canvasRect.rect.width / 2f - _edgePadding;
        float screenH = _canvasRect.rect.height / 2f - _edgePadding;

        bool isOffScreen = screenPoint.x < -screenW || screenPoint.x > screenW
                        || screenPoint.y < -screenH || screenPoint.y > screenH;

        bool isTooClose = distance < hideDistance;

        gameObject.SetActive(isOffScreen && !isTooClose);

        if (!isOffScreen || isTooClose) return;

        Vector3 dir = new Vector3(screenPoint.x, screenPoint.y, 0).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _rect.rotation = Quaternion.Euler(0, 0, angle);

        float slope = screenH / screenW;
        float absY = Mathf.Abs(dir.y);
        float absX = Mathf.Abs(dir.x);

        Vector2 edgePos;
        if (absY <= slope * absX)
            edgePos = new Vector2(Mathf.Sign(dir.x) * screenW, dir.y / absX * screenW);
        else
            edgePos = new Vector2(dir.x / absY * screenH, Mathf.Sign(dir.y) * screenH);

        _rect.anchoredPosition = edgePos;
    }
}