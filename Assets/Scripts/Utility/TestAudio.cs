using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestAudio : MonoBehaviour
{
    public AudioManager audioManager;

#if UNITY_EDITOR

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.tKey.wasPressedThisFrame)
        {
            audioManager.PlayAudio(AudioType.SFX_01, true);
        }
        if (keyboard.gKey.wasPressedThisFrame)
        {
            audioManager.StopAudio(AudioType.SFX_01, true);
        }
        if (keyboard.bKey.wasPressedThisFrame)
        {
            audioManager.RestartAudio(AudioType.SFX_01, true);
        }
        if (keyboard.yKey.wasPressedThisFrame)
        {
            audioManager.PlayAudio(AudioType.SFX_01, true);
        }
        if (keyboard.hKey.wasPressedThisFrame)
        {
            audioManager.StopAudio(AudioType.SFX_01, true);
        }
        if (keyboard.nKey.wasPressedThisFrame)
        {
            audioManager.RestartAudio(AudioType.SFX_01, true);
        }
    }

#endif

}
