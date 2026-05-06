// ObjectPool.cs
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        GameObject obj;

        if (pool[prefab].Count > 0)
        {
            obj = pool[prefab].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, position, rotation);
            PooledObject pooled = obj.AddComponent<PooledObject>();
            pooled.SetPrefab(prefab);
        }

        return obj;
    }
    public void Return(GameObject prefab, GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);

        if (!pool.ContainsKey(prefab))
            pool[prefab] = new Queue<GameObject>();

        pool[prefab].Enqueue(obj);
    }


}
