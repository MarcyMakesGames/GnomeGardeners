using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;

    private SoundEffect[] soundEffects;
    private AudioSource[] audioSources;
    private AudioSource soundSource;
    private AudioSource musicSource;
    private AudioSource ambienceSource;

    public AudioMixer audioMixer;

    // temp: set masterVolume to 1 on build
    private float masterVolume = 0.2f;
    private float soundVolume = 1f;
    private float musicVolume = 0.5f;
    private float ambienceVolume = 0.5f;

    public float MasterVolume { get => masterVolume; set => UpdateMasterVolume(value); }
    public float SoundVolume { get => soundVolume; set => UpdateSoundVolume(value); }
    public float MusicVolume { get => musicVolume; set => UpdateMusicVolume(value); }
    public float AmbienceVolume { get => ambienceVolume; set => UpdateAmbienceVolume(value); }
    public void PlaySound(AudioClip clipToPlay) => soundSource.PlayOneShot(clipToPlay);
    public bool PlayingAmbience { get => ambienceSource.isPlaying; }
    public AudioClip CurrentBGM { get => musicSource.clip; }

    #region Unity Methods

    private void Awake()
    {
        if(GameManager.Instance.AudioManager == null)
        {
            GameManager.Instance.AudioManager = this;
        }

        audioSources = GetComponents<AudioSource>();
        soundSource = audioSources[0];
        musicSource = audioSources[1];
        ambienceSource = audioSources[2];
    }

    private void Start()
    {
        soundEffects = Resources.LoadAll("Sound Effects", typeof(SoundEffect)).Cast<SoundEffect>().ToArray();

        PlayMusic(bgm, true);
        musicSource.loop = true;
        ambienceSource.loop = true;
        UpdateMasterVolume(masterVolume);
        UpdateSoundVolume(soundVolume);
        UpdateMusicVolume(musicVolume);
        UpdateAmbienceVolume(ambienceVolume);

        MasterVolume = GameManager.Instance.ConfigController.MasterVolume;
        SoundVolume = GameManager.Instance.ConfigController.SoundVolume;
        MusicVolume = GameManager.Instance.ConfigController.MusicVolume;
    }

    #endregion

    #region Public Methods

    public void PlayMusic(AudioClip clipToPlay, bool fade = false)
    {
        if (musicSource.clip == clipToPlay) { return; }
        if (fade)
            StartCoroutine(StartFade(1f, 1f));
        musicSource.clip = clipToPlay;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (!musicSource.isPlaying) { return; }
        musicSource.Stop();
    }

    public void PlayAmbience(SoundType type, bool fade = false)
    {
        SoundEffect soundEffectToPlay = null;
        foreach (SoundEffect sound in soundEffects)
        {
            if (sound.type == type)
            {
                soundEffectToPlay = sound;
            }
        }
        if(soundEffectToPlay != null)
        {
            if (fade)
                StartCoroutine(StartFade(1f, 1f));
            ambienceSource.clip = soundEffectToPlay.GetRandomClip();
            ambienceSource.Play();
        }
    }
    public void PlayAmbience(SoundType type, AudioSource source, bool fade = false)
    {
        SoundEffect soundEffectToPlay = null;
        foreach (SoundEffect sound in soundEffects)
        {
            if (sound.type == type)
            {
                soundEffectToPlay = sound;
            }
        }
        if (soundEffectToPlay != null)
        {
            source.volume = ambienceVolume * masterVolume;
            source.clip = soundEffectToPlay.GetRandomClip();
            source.Play();
        }
    }

    public void StopAmbience()
    {
        if (!ambienceSource.isPlaying) { return; }
        ambienceSource.Stop();
    }

    public void PlaySound(SoundType type)
    {
        SoundEffect soundEffectToPlay = null;
        foreach (SoundEffect sound in soundEffects)
        {
            if (sound.type == type)
            {
                soundEffectToPlay = sound;
            }
        }

        if(soundEffectToPlay != null)
            soundSource.PlayOneShot(soundEffectToPlay.GetRandomClip());

    }

    public void PlaySound(SoundType type, AudioSource source)
    {
        SoundEffect soundEffectToPlay = null;
        foreach (SoundEffect sound in soundEffects)
        {
            if (sound.type == type)
            {
                soundEffectToPlay = sound;
            }
        }

        if(soundEffectToPlay != null)
        {
            source.volume = soundVolume * masterVolume;
            source.clip = soundEffectToPlay.GetRandomClip();
            source.Play();
        }
    }


    #endregion

    #region Private Methods

    private void UpdateMasterVolume(float volume)
    {
        masterVolume = volume;
        soundSource.volume = soundVolume * masterVolume;
        musicSource.volume = musicVolume * masterVolume;
        ambienceSource.volume = ambienceVolume * masterVolume;
        GameManager.Instance.ConfigController.MasterVolume = volume;
    }

    private void UpdateSoundVolume(float volume)
    {
        soundVolume = volume;
        soundSource.volume = soundVolume * masterVolume;
        GameManager.Instance.ConfigController.SoundVolume = volume;
    }

    private void UpdateMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume * masterVolume;
        GameManager.Instance.ConfigController.MusicVolume = volume;
    }

    private void UpdateAmbienceVolume(float volume)
    {
        ambienceVolume = volume;
        ambienceSource.volume = ambienceVolume * masterVolume;
        GameManager.Instance.ConfigController.AmbienceVolume = volume;
    }

    private IEnumerator StartFade(float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat("MasterVolume", out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }

    #endregion
}
