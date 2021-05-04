using UnityEngine;

public class ConfigController : MonoBehaviour
{
    protected float masterVolume = 1f;
    protected float bgmVolume = 1f;
    protected float sfxVolume = 1f;

    public float MasterVolume { get => masterVolume; set => masterVolume = value; }
    public float BGMVolume { get => bgmVolume; set => bgmVolume = value; }
    public float SFXVolume { get => sfxVolume; set => sfxVolume = value; }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("Master_Volume", masterVolume);
        PlayerPrefs.SetFloat("BGM_Volume", bgmVolume);
        PlayerPrefs.SetFloat("SFX_Volume", sfxVolume);

        PlayerPrefs.Save();
    }
    private void Awake()
    {
        masterVolume = PlayerPrefs.GetFloat("Master_Volume", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGM_Volume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFX_Volume", 1f);
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
}
