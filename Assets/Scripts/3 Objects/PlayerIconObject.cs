using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    [System.Serializable]
    public class PlayerIconObject
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private List<Sprite> playerIcons;

        public int PlayerIndex { get => playerIndex; }
        public List<Sprite> PlayerIcons { get => playerIcons; }
    }
}
