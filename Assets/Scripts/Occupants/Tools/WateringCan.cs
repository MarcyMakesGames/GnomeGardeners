using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class WateringCan : Tool
    {
        #region Unity Methods



        #endregion

        #region Public Methods

        public override void Interact(Tool tool)
        {
            throw new System.NotImplementedException();

        }

        public override void UseTool(GridCell cell, GnomeController gnome)
        {
            DebugLogger.Log(this, "Executing");
            var occupant = cell.Occupant;
            if (occupant != null)
            {
                Plant plant = null;
                if (occupant.TryGetComponent(out plant))
                {
                    plant.FulfillCurrentNeed(NeedType.Water);
                }

                Insect insect = null;
                if (occupant.TryGetComponent(out insect) && occupant.TryGetComponent(out insect))
                {
                    insect.IncrementShooedCount();
                }
            }
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
