using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [SerializeField] private int maxPoolSizePerPrefab = 500;

    private Dictionary<GameObject, Queue<GameObject>> pool = new();
    private Dictionary<GameObject, int> _totalInstantiated = new();
    private Dictionary<GameObject, LinkedList<GameObject>> _activeObjects = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        if (!_activeObjects.ContainsKey(prefab))
            _activeObjects[prefab] = new LinkedList<GameObject>();

        GameObject obj;

        if (pool[prefab].Count > 0)
        {
            obj = pool[prefab].Dequeue();
        }
        else
        {
            int total = _totalInstantiated.GetValueOrDefault(prefab, 0);

            if (total >= maxPoolSizePerPrefab)
            {
                obj = ForceReturnOldest(prefab);

                if (obj == null)
                {
                    Debug.LogWarning($"[ObjectPool] Tidak ada object untuk di-reuse: {prefab.name}");
                    return null;
                }
            }
            else
            {
                obj = Instantiate(prefab, position, rotation);
                PooledObject pooled = obj.AddComponent<PooledObject>();
                pooled.SetPrefab(prefab);
                _totalInstantiated[prefab] = total + 1;
            }
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        obj.SetActive(true);
        _activeObjects[prefab].AddLast(obj);

        return obj;
    }

    public void Return(GameObject prefab, GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);

        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        pool[prefab].Enqueue(obj);

        if (_activeObjects.ContainsKey(prefab))
            _activeObjects[prefab].Remove(obj);
    }

    public void Prewarm(GameObject prefab, int count)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        if (!_activeObjects.ContainsKey(prefab))
            _activeObjects[prefab] = new LinkedList<GameObject>();

        int total = _totalInstantiated.GetValueOrDefault(prefab, 0);
        int allowed = Mathf.Min(count, maxPoolSizePerPrefab - total);

        for (int i = 0; i < allowed; i++)
        {
            GameObject obj = Instantiate(prefab);
            PooledObject pooled = obj.AddComponent<PooledObject>();
            pooled.SetPrefab(prefab);
            obj.SetActive(false);
            pool[prefab].Enqueue(obj);
        }

        _totalInstantiated[prefab] = total + allowed;
    }

    private GameObject ForceReturnOldest(GameObject prefab)
    {
        if (!_activeObjects.ContainsKey(prefab) || _activeObjects[prefab].Count == 0)
            return null;

        while (_activeObjects[prefab].Count > 0)
        {
            GameObject oldest = _activeObjects[prefab].First.Value;
            _activeObjects[prefab].RemoveFirst();

            if (oldest == null) continue;

            PooledObject pooled = oldest.GetComponent<PooledObject>();
            if (pooled != null) pooled.ReturnToPool();

            if (pool.ContainsKey(prefab) && pool[prefab].Count > 0)
                return pool[prefab].Dequeue();
        }

        return null;
    }
}