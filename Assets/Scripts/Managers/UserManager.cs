using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserManager : MonoBehaviour
{
    public GameObject playerPrefab;
    PlayerInputManager playerInputManager;
    Keyboard keyboard;
    bool keyboardLeftIsActive, keyboardRightIsActive;

    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        keyboardLeftIsActive = false;
        keyboardRightIsActive = false;

    }

    void Update()
    {
        keyboard = Keyboard.current;

        if (keyboard == null)
        {
            return;
        }

        if (keyboard.escapeKey.wasPressedThisFrame && !keyboardLeftIsActive)
        {
            PlayerInput user1 = PlayerInput.Instantiate(playerPrefab, controlScheme: "KeyboardLeft", pairWithDevice: Keyboard.current);
            keyboardLeftIsActive = true;
        }

        if (keyboard.pauseKey.wasPressedThisFrame && !keyboardRightIsActive)
        {
            PlayerInput user2 = PlayerInput.Instantiate(playerPrefab, controlScheme: "KeyboardRight", pairWithDevice: Keyboard.current);
            keyboardRightIsActive = true;
        }
    }

}
