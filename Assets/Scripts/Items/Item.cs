using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class Item
	{
        protected string name;
        protected Sprite sprite;
        protected PoolKey popUpKey;

        public string Name
        {
	        get => name;
        }
        public Sprite Sprite
        {
	        get => sprite;
        }

        public PoolKey PopUpKey
        {
	        get => popUpKey;
        }
    }
}
