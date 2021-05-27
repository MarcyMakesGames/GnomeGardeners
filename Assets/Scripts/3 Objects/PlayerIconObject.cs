using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class PlayerIconObject : MonoBehaviour
    {
        [SerializeField] private Sprite gnomeSprite;
        [SerializeField] private List<Sprite> playerIcons;
        private Sprite playerIcon;
        public Sprite SelectedPlayerIcon { get => playerIcon; }
        public List<Sprite> PlayerIcons { get => playerIcons; }
    }
}
