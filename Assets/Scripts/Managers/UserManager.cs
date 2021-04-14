using UnityEngine;
using UnityEngine.InputSystem;

public class UserManager : MonoBehaviour
{
    public GameObject playerPrefab;

    private Keyboard keyboard;
    private bool keyboardLeftIsActive, keyboardRightIsActive;

    #region Unity Methods

    void Start()
    {
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
    #endregion
}
