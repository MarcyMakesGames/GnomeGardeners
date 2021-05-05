using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] protected AudioClip bgm;
    protected AudioSource[] audioSources;
    protected AudioSource sfxAudioSource;
    protected AudioSource bgmAudioSource;
    protected AudioSource ambienceSource;

    protected float masterVolume = 1f;
    protected float sfxAudioVolume = 1f;
    protected float bgmAudioVolume = 1f;

    public float MasterVolume { get => masterVolume; set => UpdateMasterVolume(value); }
    public float SFXVolume { get => SFXVolume; set => UpdateSFXVolume(value); }
    public float BGMVolume { get => BGMVolume; set => UpdateBGMVolume(value); }
    public void PlaySound(AudioClip clipToPlay) => sfxAudioSource.PlayOneShot(clipToPlay);
    public bool PlayingAmbience { get => ambienceSource.isPlaying; }
    public AudioClip CurrentBGM { get => bgmAudioSource.clip; }

    public void PlayMusic(AudioClip clipToPlay)
    {
        if (bgmAudioSource.clip == clipToPlay)
            return;

        bgmAudioSource.clip = clipToPlay;
        bgmAudioSource.Play();
    }

    public void PlayAmbience(AudioClip clipToPlay)
    {
        ambienceSource.clip = clipToPlay;
        ambienceSource.Play();
    }

    protected void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        sfxAudioSource = audioSources[0];
        bgmAudioSource = audioSources[1];
        ambienceSource = audioSources[2];
    }

    protected void Start()
    {
        PlayMusic(bgm);
        bgmAudioSource.loop = true;
        ambienceSource.loop = true;

        MasterVolume = GameManager.Instance.Config.MasterVolume;
        SFXVolume = GameManager.Instance.Config.SFXVolume;
        BGMVolume = GameManager.Instance.Config.BGMVolume;
        ambienceSource.volume = sfxAudioVolume;
    }

    protected void UpdateMasterVolume(float value)
    {
        masterVolume = value;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;
        GameManager.Instance.Config.MasterVolume = value;
    }

    protected void UpdateSFXVolume(float value)
    {
        sfxAudioVolume = value;
        sfxAudioSource.volume = sfxAudioVolume * masterVolume;
        GameManager.Instance.Config.SFXVolume = value;

        ambienceSource.volume = sfxAudioVolume * masterVolume;
    }

    protected void UpdateBGMVolume(float value)
    {
        bgmAudioVolume = value;
        bgmAudioSource.volume = bgmAudioVolume * masterVolume;
        GameManager.Instance.Config.BGMVolume = value;
    }

}
