using UnityEngine;
using System.Collections.Generic;

public class EnemyIndicatorManager : MonoBehaviour
{
    public static EnemyIndicatorManager Instance { get; private set; }

    [SerializeField] private GameObject indicatorPrefab;

    private Canvas _canvas;
    private Dictionary<Transform, EnemyIndicator> _indicators = new();

    void Awake()
    {
        Instance = this;
        _canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        var toRemove = new List<Transform>();

        foreach (var kvp in _indicators)
        {
            if (kvp.Key == null)
            {
                toRemove.Add(kvp.Key);
                continue;
            }
            kvp.Value.UpdateIndicator();
        }

        foreach (var t in toRemove)
        {
            Destroy(_indicators[t].gameObject);
            _indicators.Remove(t);
        }
    }

    public void RegisterEnemy(Transform enemy)
    {
        if (_indicators.ContainsKey(enemy)) return;

        var go = Instantiate(indicatorPrefab, _canvas.transform);
        var indicator = go.GetComponent<EnemyIndicator>();
        indicator.Init(enemy);
        _indicators[enemy] = indicator;
    }

    public void UnregisterEnemy(Transform enemy)
    {
        if (!_indicators.ContainsKey(enemy)) return;

        Destroy(_indicators[enemy].gameObject);
        _indicators.Remove(enemy);
    }
}