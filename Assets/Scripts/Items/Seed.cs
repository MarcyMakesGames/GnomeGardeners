using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class Seed : Item
	{
        public PoolKey plantKey;
        public GameObject prefab;

        public Seed(PoolKey _plantKey, PoolKey _popUpKey)
        {
            name = "Seed";
            sprite = Resources.Load<Sprite>("seed");
            plantKey = _plantKey;
            popUpKey = _popUpKey;
            switch (_plantKey)
            {
                case PoolKey.Plant_Daffodil:
                    prefab = Resources.Load<GameObject>("Plants/Daffodil");
                    break;
                case PoolKey.Plant_Peppermint:
                    prefab = Resources.Load<GameObject>("Plants/Peppermint");
                    break;
                case PoolKey.Plant_Strawberry:
                    prefab = Resources.Load<GameObject>("Plants/Strawberry");
                    break;
                case PoolKey.Plant_Sunflower:
                    prefab = Resources.Load<GameObject>("Plants/Sunflower");
                    break;
                case PoolKey.Plant_Tulip:
                    prefab = Resources.Load<GameObject>("Plants/Tulip");
                    break;
            }
        }
    }
}
