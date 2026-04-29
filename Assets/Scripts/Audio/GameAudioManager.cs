using UnityEngine;

/// <summary>
/// Singleton audio manager present in every scene.
/// Each sound has its own individual volume slider in the Inspector.
/// </summary>
public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;

    [Header("Music")]
    [SerializeField] private AudioClip _musicClip;
    [SerializeField] private bool _playMusicOnStart;
    [SerializeField, Range(0f, 3f)] private float _musicVolume = 1f;

    [Header("UI")]
    [SerializeField] private AudioClip _click1;
    [SerializeField, Range(0f, 15f)] private float _click1Volume = 1f;

    [Header("Lvl1 Sounds")]
    [SerializeField] private AudioClip _damage;
    [SerializeField, Range(0f, 3f)] private float _damageVolume = 1f;
    [SerializeField] private AudioClip _heal;
    [SerializeField, Range(0f, 3f)] private float _healVolume = 1f;
    [SerializeField] private AudioClip _loose;
    [SerializeField, Range(0f, 3f)] private float _looseVolume = 1f;
    [SerializeField] private AudioClip _ram;
    [SerializeField, Range(0f, 3f)] private float _ramVolume = 1f;

    [Header("Lvl2 Sounds")]
    [SerializeField] private AudioClip _click2;
    [SerializeField, Range(0f, 15f)] private float _click2Volume = 1f;
    [SerializeField] private AudioClip _click3;
    [SerializeField, Range(0f, 15f)] private float _click3Volume = 1f;
    [SerializeField] private AudioClip _click4;
    [SerializeField, Range(0f, 15f)] private float _click4Volume = 1f;
    [SerializeField] private AudioClip _click5;
    [SerializeField, Range(0f, 15f)] private float _click5Volume = 1f;
    [SerializeField] private AudioClip _looseLvl2;
    [SerializeField, Range(0f, 3f)] private float _looseLvl2Volume = 1f;

    [Header("Lvl3 Sounds")]
    [SerializeField] private AudioClip _looseLvl3;
    [SerializeField, Range(0f, 3f)] private float _looseLvl3Volume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (_playMusicOnStart && _musicClip != null)
            PlayMusic();
    }

    /// <summary>Plays a one-shot SFX clip at the given volume.</summary>
    public void PlaySFX(AudioClip clip, float volume)
    {
        if (clip == null || _sfxSource == null) return;
        _sfxSource.PlayOneShot(clip, volume);
    }

    /// <summary>Starts the looping background music.</summary>
    public void PlayMusic()
    {
        if (_musicSource == null || _musicClip == null) return;
        _musicSource.clip = _musicClip;
        _musicSource.volume = _musicVolume;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    /// <summary>Stops the background music.</summary>
    public void StopMusic()
    {
        if (_musicSource == null) return;
        _musicSource.Stop();
    }

    // --- Convenience methods ---

    public void PlayClick()            => PlaySFX(_click1, _click1Volume);
    public void PlayDamage()           => PlaySFX(_damage, _damageVolume);
    public void PlayHeal()             => PlaySFX(_heal, _healVolume);
    public void PlayLoose()            => PlaySFX(_loose, _looseVolume);
    public void PlayRam()              => PlaySFX(_ram, _ramVolume);
    public void PlayLooseLvl2()        => PlaySFX(_looseLvl2, _looseLvl2Volume);
    public void PlayLooseLvl3()        => PlaySFX(_looseLvl3, _looseLvl3Volume);

    /// <summary>Plays the escalating click sound for the ClickPopup (1-indexed).</summary>
    public void PlayClickEscalating(int clickNumber)
    {
        switch (clickNumber)
        {
            case 1: PlaySFX(_click1, _click1Volume); break;
            case 2: PlaySFX(_click2, _click2Volume); break;
            case 3: PlaySFX(_click3, _click3Volume); break;
            case 4: PlaySFX(_click4, _click4Volume); break;
            default: PlaySFX(_click5, _click5Volume); break;
        }
    }
}
