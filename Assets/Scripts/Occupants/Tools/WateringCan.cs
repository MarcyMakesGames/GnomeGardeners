using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

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

        public override void UseTool(GridCell cell)
        {
            DebugLogger.Log(this, "Executing");
            var occupant = cell.Occupant;
            if (occupant != null)
            {
                Plant plant = null;
                if (occupant.TryGetComponent(out plant))
                {
                    plant.FulfillCurrentNeed(NeedType.Water);
                    return;
                }

                Insect insect = null;
                if (occupant.TryGetComponent(out insect) && occupant.TryGetComponent(out insect))
                {
                    insect.IncrementShooedCount();
                    return;
                }

                occupant.FailedInteraction();
            }
        }
        public override void UpdateSpriteResolvers(SpriteResolver[] resolvers)
        {
            foreach (SpriteResolver resolver in resolvers)
            {
                resolver.SetCategoryAndLabel("tools", "water");
            }
        }

        public override void FailedInteraction()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
