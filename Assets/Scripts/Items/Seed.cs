using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class Seed : Item
	{
        public PoolKey plantKey;

        public Seed(PoolKey key)
        {
            name = "Seed";
            sprite = Resources.Load<Sprite>("seed");
            plantKey = key;
            popUpKey = PoolKey.PopUp_Seed;
        }
    }
}
