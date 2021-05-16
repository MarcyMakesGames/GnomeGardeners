using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Compost : Occupant
    {
        private readonly bool debug = false;

        public GameObject dispensable;

        #region Unity Methods



        #endregion

        #region Public Methods

        public override void Interact(Tool tool)
        {

        }

        public IHoldable DispenseItem(Tool tool)
        {
            DebugLogger.Log(this, "Dispensing." + dispensable.name);
            return dispensable.GetComponent<IHoldable>();
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
