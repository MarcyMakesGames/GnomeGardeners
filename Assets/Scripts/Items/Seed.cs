using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class Seed : Item
	{
        public PoolKey plantKey;

        public Seed(PoolKey _plantKey, PoolKey _popUpKey)
        {
            name = "Seed";
            sprite = Resources.Load<Sprite>("seed");
            plantKey = _plantKey;
            popUpKey = _popUpKey;
        }
    }
}
