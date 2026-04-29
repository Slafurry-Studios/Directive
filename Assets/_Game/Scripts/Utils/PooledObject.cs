// PooledObject.cs
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private GameObject prefab;

    public void SetPrefab(GameObject p) => prefab = p;
    public GameObject Prefab => prefab;

    public void ReturnToPool()
    {
        if (ObjectPool.Instance != null)
            ObjectPool.Instance.Return(prefab, gameObject);
        else
            Destroy(gameObject);
    }
}