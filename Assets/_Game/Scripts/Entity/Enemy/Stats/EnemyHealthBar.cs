using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public Slider slider;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetHealth(float current, float max)
    {
        slider.value = current / max;
    }

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
        transform.rotation = Quaternion.identity;
    }
}