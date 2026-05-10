using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Collectibles : MonoBehaviour
{
    [SerializeField] private int effect;
    [SerializeField] private COLLECTIBLE_TYPE type = COLLECTIBLE_TYPE.HEALTH;
    void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.CompareTag("Player"))
        {
            if (type == COLLECTIBLE_TYPE.HEALTH) entity.GetComponentInChildren<PlayerHealth>().Heal(effect);
            if (type == COLLECTIBLE_TYPE.ENERGY) entity.GetComponentInChildren<PlayerEnergy>().RegainEnergy(effect);

            Destroy(gameObject);
        }
    }
}

public enum COLLECTIBLE_TYPE {
    HEALTH,
    ENERGY
}