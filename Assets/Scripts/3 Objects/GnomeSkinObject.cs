using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    [System.Serializable]
    public class GnomeSkinObject
    {
        [SerializeField] private int gnomeSkin;
        [SerializeField] private Sprite gnomeSprite;

        public int GnomeSkin { get => gnomeSkin; }
        public Sprite GnomeSprite { get => gnomeSprite; }
    }
}
