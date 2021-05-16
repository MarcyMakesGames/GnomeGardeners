using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Fertilizer : Item
    {
        public ItemType type = ItemType.Fertilizer;

        [SerializeField] private float strength = 50f;

        public float Strength { get => strength; }

        public Fertilizer()
        {
            name = "Fertilizer";
            sprite = Resources.Load<Sprite>("compost_icon");
        }
    }
}
