using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class GnomeMenuSetupController : MonoBehaviour
{
    public GameObject setupMenuPrefab;
    public PlayerInput input;

    private GridLayoutGroup menuLayout;

    private void Start()
    {
        menuLayout = FindObjectOfType<GridLayoutGroup>();

        if(menuLayout != null)
        {
            GameObject menu = Instantiate(setupMenuPrefab, menuLayout.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();

            var actionMap = new GnomeInput();
            input.uiInputModule.actionsAsset = actionMap.asset;

            menu.GetComponentInChildren<GnomeMenuController>().SetPlayerIndex(input.playerIndex);
        }        
    }
}
