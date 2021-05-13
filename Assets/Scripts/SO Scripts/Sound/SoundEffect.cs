using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Effect", menuName = "Sounds/Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public SoundType type;
    public AudioClip[] sounds;

    public AudioClip GetRandomClip()
    {
        var length = sounds.Length;
        var randomIndex = UnityEngine.Random.Range(0, length);
        return sounds[randomIndex];
    }
}
