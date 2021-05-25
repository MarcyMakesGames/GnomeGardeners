using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Fertilizer : Item
    {
        public ItemType type = ItemType.Fertilizer;

        public Fertilizer()
        {
            name = "Fertilizer";
            sprite = Resources.Load<Sprite>("compost_icon");
            popUpKey = PoolKey.PopUp_Fertilizer;
        }
    }
}
