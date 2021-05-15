using UnityEngine;

public class ConfigController : MonoBehaviour
{
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float soundVolume = 1f;
    private float ambienceVolume = 1f;

    public float MasterVolume { get => masterVolume; set => masterVolume = value; }
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
    public float SoundVolume { get => soundVolume; set => soundVolume = value; }
    public float AmbienceVolume { get => ambienceVolume; set => ambienceVolume = value; }

    private void Awake()
    {
        if (GameManager.Instance.ConfigController == null)
        {
            GameManager.Instance.ConfigController = this;
        }

        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", 1f);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        PlayerPrefs.SetFloat("AmbienceVolume", ambienceVolume);

        PlayerPrefs.Save();
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
}
