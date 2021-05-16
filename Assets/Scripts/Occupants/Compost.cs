using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Compost : Occupant
    {
        private readonly bool debug = false;

        public Item dispensable;

        #region Unity Methods

        private new void Start()
        {
            base.Start();
            dispensable = new Fertilizer();
        }

        #endregion

        #region Public Methods

        public override void Interact(Tool tool)
        {

        }

        public Item DispenseItem()
        {
            DebugLogger.Log(this, "Dispensing." + dispensable.Name);
            return dispensable;
        }

        public void DispenseItem(Vector2Int dropLocation)
        {
            throw new System.NotImplementedException();
        }

        public void AddScore(int score)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
