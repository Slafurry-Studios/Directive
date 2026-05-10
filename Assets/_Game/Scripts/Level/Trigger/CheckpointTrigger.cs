using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private int checkpointID = 0;
    void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("PlayerCheckpoint", checkpointID);
            PlayerPrefs.Save();
        }
    }

    public int CheckPointID => checkpointID;
}