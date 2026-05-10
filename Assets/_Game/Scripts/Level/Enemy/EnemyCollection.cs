using System.Collections.Generic;
using UnityEngine;

public class EnemyCollection : MonoBehaviour
{
    [SerializeField] GameObject[] objectToActivate;
    [SerializeField] GameObject[] objectToDeactivate;

    private List<Enemy> enemies = new List<Enemy>();

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

    private void OnEnable()
    {
        enemies.Clear();

        var found = GetComponentsInChildren<Enemy>(true);

        foreach (var enemy in found)
        {
            AddEnemy(enemy);
        }

    }

    private void AddEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {

        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);

            if (enemies.Count <= 0)
            {
                Cleared();
            }
        }
    }

    private void Cleared()
    {
        Debug.Log($"[EnemyCollection] {gameObject.name} has been cleared!");

        foreach (var obj in objectToActivate)
        {
            Debug.Log($"[EnemyCollection] {gameObject.name} has activating {obj.name}");
            if (obj != null) obj.SetActive(true);
        }

        foreach (var obj in objectToDeactivate)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

}