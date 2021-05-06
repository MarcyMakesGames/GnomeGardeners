using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;
    private AudioSource[] audioSources;
    private AudioSource sfxAudioSource;
    private AudioSource bgmAudioSource;
    private AudioSource ambienceSource;

    private float masterVolume = 1f;
    private float sfxAudioVolume = 1f;
    private float bgmAudioVolume = 1f;

    public float MasterVolume { get => masterVolume; set => UpdateMasterVolume(value); }
    public float SFXVolume { get => SFXVolume; set => UpdateSFXVolume(value); }
    public float BGMVolume { get => BGMVolume; set => UpdateBGMVolume(value); }
    public void PlaySound(AudioClip clipToPlay) => sfxAudioSource.PlayOneShot(clipToPlay);
    public bool PlayingAmbience { get => ambienceSource.isPlaying; }
    public AudioClip CurrentBGM { get => bgmAudioSource.clip; }

    #region Unity Methods

    private void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        sfxAudioSource = audioSources[0];
        bgmAudioSource = audioSources[1];
        ambienceSource = audioSources[2];
    }

    private void Start()
    {
        PlayMusic(bgm);
        bgmAudioSource.loop = true;
        ambienceSource.loop = true;

        MasterVolume = GameManager.Instance.Config.MasterVolume;
        SFXVolume = GameManager.Instance.Config.SFXVolume;
        BGMVolume = GameManager.Instance.Config.BGMVolume;
        ambienceSource.volume = sfxAudioVolume;
    }

    #endregion

    #region Public Methods

    public void PlayMusic(AudioClip clipToPlay)
    {
        if (bgmAudioSource.clip == clipToPlay) { return; }
        bgmAudioSource.clip = clipToPlay;
        bgmAudioSource.Play();
    }

    public void StopMusic()
    {
        if (!bgmAudioSource.isPlaying) { return; }
        bgmAudioSource.Stop();
    }

    public void PlayAmbience(AudioClip clipToPlay)
    {
        if(ambienceSource.clip == clipToPlay) { return; }
        ambienceSource.clip = clipToPlay;
        ambienceSource.Play();
    }

    public void StopAmbience()
    {
        if (!ambienceSource.isPlaying) { return; }
        ambienceSource.Stop();
    }

    #endregion

    #region Private Methods

    private void UpdateMasterVolume(float value)
    {
        masterVolume = value;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;
        GameManager.Instance.Config.MasterVolume = value;
    }

    private void UpdateSFXVolume(float value)
    {
        sfxAudioVolume = value;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;
        GameManager.Instance.Config.SFXVolume = value;

        ambienceSource.volume = sfxAudioVolume * masterVolume;
    }

    private void UpdateBGMVolume(float value)
    {
        bgmAudioVolume = value;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;
        GameManager.Instance.Config.BGMVolume = value;
    }

    #endregion
}
