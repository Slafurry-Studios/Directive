using UnityEngine;
using System.Collections;

public class ChokeCutscene : MonoBehaviour
{
    [Header("Actors")]
    [SerializeField] private SpriteRenderer attackerSprite;
    [SerializeField] private SpriteRenderer victimSprite;
    [SerializeField] private Transform attacker;
    [SerializeField] private Transform victim;

    [Header("Attacker Sprites")]
    [SerializeField] private Sprite[] attackerMoveFrames;
    [SerializeField] private Sprite attackerChokeSprite;

    [Header("Victim Sprites")]
    [SerializeField] private Sprite victimChokedSprite;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float animFps = 12f;

    [Header("Audio")]
    [SerializeField] private AudioClip sfxMove;
    [SerializeField] private AudioClip sfxChoke;
    [Range(0f, 10f)] [SerializeField] private float sfxMoveVolume = 1f;
    [Range(0f, 10f)] [SerializeField] private float sfxChokeVolume = 1f;

    [Header("On Finish")]
    [SerializeField] private GameObject objectToActivate;

    private Coroutine moveAnimCoroutine;

    void OnEnable()
    {
        StartCoroutine(PlayCutscene());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        moveAnimCoroutine = null;
    }

    IEnumerator PlayCutscene()
    {
        if (attacker == null || victim == null || attackerSprite == null || victimSprite == null)
        {
            Debug.LogWarning("[ChokeCutscene] Referensi kosong — cek Inspector!");
            yield break;
        }

        yield return null; // tunggu 1 frame agar audio system siap

        FaceTarget(attacker, victim.position);
        FaceTarget(victim, attacker.position);

        PlaySfx(sfxMove, sfxMoveVolume);

        if (attackerMoveFrames != null && attackerMoveFrames.Length > 0)
            moveAnimCoroutine = StartCoroutine(PlaySpriteLoop(attackerSprite, attackerMoveFrames, animFps));

        while (Vector2.Distance(attacker.position, victim.position) > stopDistance)
        {
            attacker.position = Vector2.MoveTowards(
                attacker.position,
                victim.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        if (moveAnimCoroutine != null)
        {
            StopCoroutine(moveAnimCoroutine);
            moveAnimCoroutine = null;
        }

        if (attackerChokeSprite != null)
            attackerSprite.sprite = attackerChokeSprite;

        if (victimChokedSprite != null)
            victimSprite.sprite = victimChokedSprite;

        PlaySfx(sfxChoke, sfxChokeVolume);

        yield return new WaitForSeconds(1f);

        if (objectToActivate != null)
            objectToActivate.SetActive(true);
    }

    IEnumerator PlaySpriteLoop(SpriteRenderer sr, Sprite[] frames, float fps)
    {
        float interval = 1f / Mathf.Max(fps, 0.01f);
        while (true)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                sr.sprite = frames[i];
                yield return new WaitForSeconds(interval);
            }
        }
    }

    void FaceTarget(Transform self, Vector3 targetPos)
    {
        Vector2 dir = (targetPos - self.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.down, dir);
        self.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void PlaySfx(AudioClip clip, float volume)
    {
        Debug.Log($"[ChokeCutscene] PlaySfx — clip={clip?.name}, volume={volume}, instance={SfxPlayer.Instance}");
        if (clip != null && SfxPlayer.Instance != null)
            SfxPlayer.Instance.PlayUISfx(clip, volume);
    }
}