using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [Range(0f, 10f)]
    [SerializeField] private float audioVolume;
    void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.CompareTag("Player"))
        {
            MusicPlayer.Instance.PlayMusic(audioClip, audioVolume);
        }
    }
}