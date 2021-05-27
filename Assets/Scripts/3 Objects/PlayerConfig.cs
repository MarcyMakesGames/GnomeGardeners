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
        public GnomeSkinObject GnomeSkin { get; set; }
        public PlayerIconObject PlayerIcon { get; set; }
        public bool IsReady { get; set; }


        public PlayerConfig(PlayerInput playerInput)
        {
            PlayerIndex = playerInput.playerIndex;
            Input = playerInput;
            IsReady = false;
        }
    }
}
