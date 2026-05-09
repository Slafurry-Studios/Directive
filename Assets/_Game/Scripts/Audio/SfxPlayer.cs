using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer Instance { get; private set; }

    public static event System.Action OnReady;

    [Header("Pool Sizes")]
    [SerializeField] private int _uiPoolSize = 5;
    [SerializeField] private int _playerPoolSize = 10;
    [SerializeField] private int _enemyPoolSize = 15;
    [SerializeField] private int _environmentPoolSize = 10;
    [SerializeField] private int _bulletPoolSize = 20;

    private AudioSource[] _uiPool;
    private AudioSource[] _playerPool;
    private AudioSource[] _enemyPool;
    private AudioSource[] _environmentPool;
    private AudioSource[] _bulletPool;

    private const string SFX_VOLUME_KEY = "SfxVolume";
    private float _sfxVolume = 1f;

    private readonly Dictionary<AudioClip, AudioSource> _loopingSources = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _uiPool = CreatePool("UI", _uiPoolSize);
        _playerPool = CreatePool("Player", _playerPoolSize);
        _enemyPool = CreatePool("Enemy", _enemyPoolSize);
        _environmentPool = CreatePool("Environment", _environmentPoolSize);
        _bulletPool = CreatePool("Bullet", _bulletPoolSize);
    }

    void Start()
    {
        _sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        OnReady?.Invoke();
    }

    // ─── Public Playback ──────────────────────────────────────────────────────

    public void PlayUISfx(AudioClip clip, float volume = 1f, bool loop = false, int priority = 16)
        => PlayFromPool(_uiPool, clip, volume, loop, priority);

    public void PlayPlayerSfx(AudioClip clip, float volume = 1f, bool loop = false, int priority = 32)
        => PlayFromPool(_playerPool, clip, volume, loop, priority);

    public void PlayEnemySfx(AudioClip clip, float volume = 1f, bool loop = false, int priority = 64)
        => PlayFromPool(_enemyPool, clip, volume, loop, priority);

    public void PlayEnvironmentSfx(AudioClip clip, float volume = 1f, bool loop = false, int priority = 128)
        => PlayFromPool(_environmentPool, clip, volume, loop, priority);

    public void PlayBulletSfx(AudioClip clip, float volume = 1f, bool loop = false, int priority = 200)
        => PlayFromPool(_bulletPool, clip, volume, loop, priority);

    // ─── Loop Control ─────────────────────────────────────────────────────────

    public void StopLoopingSfx(AudioClip clip)
    {
        if (clip == null) return;
        if (_loopingSources.TryGetValue(clip, out AudioSource source))
        {
            source.loop = false;
            source.Stop();
            _loopingSources.Remove(clip);
        }
    }

    public void StopAllLoopingSfx()
    {
        foreach (var kvp in _loopingSources)
        {
            kvp.Value.loop = false;
            kvp.Value.Stop();
        }
        _loopingSources.Clear();
    }

    public void SetLoopingVolume(AudioClip clip, float volume)
    {
        if (clip == null) return;
        if (_loopingSources.TryGetValue(clip, out AudioSource source))
            source.volume = Mathf.Clamp01(volume * _sfxVolume);
    }

    // ─── Volume ───────────────────────────────────────────────────────────────

    public void SetSfxVolume(float value)
    {
        _sfxVolume = value;
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    public float GetSfxVolume() => _sfxVolume;

    // ─── Internal ─────────────────────────────────────────────────────────────

    private void PlayFromPool(AudioSource[] pool, AudioClip clip, float volume, bool loop, int priority = 128)
    {
        if (clip == null) return;

        float finalVolume = Mathf.Clamp01(volume * _sfxVolume);

        if (loop)
        {
            if (_loopingSources.ContainsKey(clip)) return;
            AudioSource source = GetAvailableSource(pool);
            source.clip = clip;
            source.volume = finalVolume;
            source.priority = priority;
            source.loop = true;
            source.Play();
            _loopingSources[clip] = source;
        }
        else
        {
            int sameClipCount = 0;
            foreach (AudioSource source in pool)
                if (source.isPlaying && source.clip == clip)
                    sameClipCount++;

            // if (sameClipCount >= 3) return;

            AudioSource available = null;
            foreach (AudioSource source in pool)
            {
                if (!source.isPlaying)
                {
                    available = source;
                    break;
                }
            }

            if (available == null)
            {
                available = pool[0];
                foreach (AudioSource source in pool)
                    if (source.time > available.time)
                        available = source;
            }

            available.clip = clip;
            available.volume = finalVolume;
            available.priority = priority;
            available.loop = false;
            available.Play();
        }
    }

    private AudioSource GetAvailableSource(AudioSource[] pool)
    {
        foreach (AudioSource source in pool)
            if (!source.isPlaying) return source;

        AudioSource oldest = pool[0];
        foreach (AudioSource source in pool)
            if (source.time > oldest.time)
                oldest = source;

        return oldest;
    }

    private AudioSource[] CreatePool(string groupName, int size)
    {
        GameObject parent = new GameObject($"Pool_{groupName}");
        parent.transform.SetParent(transform);

        AudioSource[] pool = new AudioSource[size];
        for (int i = 0; i < size; i++)
        {
            GameObject go = new GameObject($"Source_{i}");
            go.transform.SetParent(parent.transform);
            pool[i] = go.AddComponent<AudioSource>();
            pool[i].playOnAwake = false;
            pool[i].priority = 128; // default
        }
        return pool;
    }
}