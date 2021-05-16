using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class Harvest : Item
	{
        public int points;

        public Harvest(int _points)
        {
            name = "Harvest";
            sprite = null;
            points = _points;
        }
    }
}
