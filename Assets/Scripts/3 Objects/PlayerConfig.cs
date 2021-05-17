using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GnomeGardeners
{
    public class PlayerConfig
    {
        public PlayerInput Input { get; set; }
        public int PlayerIndex { get; set; }
        public bool IsReady { get; set; }

        //We need to add some kind of ISkin or object to represent the gnome appearance here.

        public PlayerConfig(PlayerInput playerInput)
        {
            PlayerIndex = playerInput.playerIndex;
            Input = playerInput;
            IsReady = false;
        }
    }
}
