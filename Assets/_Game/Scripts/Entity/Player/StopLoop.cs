using UnityEngine;

public class StopLoop : MonoBehaviour
{
    [SerializeField] private bool isMusicToo = false;
    void Start()
    {
        SfxPlayer.Instance.StopAllLoopingSfx();
        if (isMusicToo) MusicPlayer.Instance.StopMusic();
        Debug.Log("Clear sfx");
    }
}