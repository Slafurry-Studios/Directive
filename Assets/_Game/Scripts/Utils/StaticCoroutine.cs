using UnityEngine;
using System.Collections;

public class StaticCoroutine : MonoBehaviour
{
    private static StaticCoroutine _instance;

    private static StaticCoroutine Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("[StaticCoroutine]");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<StaticCoroutine>();
            }
            return _instance;
        }
    }

    public static void Run(IEnumerator routine)
    {
        Instance.StartCoroutine(routine);
    }
}