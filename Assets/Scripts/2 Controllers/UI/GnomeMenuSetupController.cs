using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace GnomeGardeners
{
    public class GnomeMenuSetupController : MonoBehaviour
    {
        public GameObject setupMenuPrefab;
        public PlayerInput input;

        private GameObject menuLayout;

        private void Awake()
        {
            menuLayout = GameObject.Find("Gnome Options Panel");

            if (menuLayout != null)
            {
                var menu = Instantiate(setupMenuPrefab, menuLayout.transform);
                input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
                menu.GetComponent<GnomeMenuController>().SetPlayerIndex(input.playerIndex);
            }
        }
    }
}
