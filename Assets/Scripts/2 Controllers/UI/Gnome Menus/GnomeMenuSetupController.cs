using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace GnomeGardeners
{
    public class GnomeMenuSetupController : MonoBehaviour
    {
        public PlayerInput input;

        private GameObject menuLayout;

        private void Awake()
        {
            menuLayout = GameObject.Find("Player Setup Menus");

            if (menuLayout != null)
            {
                var menu = GameObject.FindGameObjectWithTag("Player" + input.playerIndex + 1.ToString() + "Menu");
                menu.GetComponent<GnomeMenuController>().PlayerIndex = input.playerIndex;
                input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            }
        }
    }
}
