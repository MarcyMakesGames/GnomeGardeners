using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

namespace GnomeGardeners
{
    public class Hoe : Tool
    {
        public override void Interact(Tool tool)
        {
            throw new System.NotImplementedException();
        }

        #region Unity Methods



        #endregion

        #region Public Methods

        public override void UseTool(GridCell cell)
        {
            DebugLogger.Log(this, "Executing");
            var occupant = cell.Occupant;
            if (occupant != null)
            {
                DebugLogger.Log(this, "Occupant found!");
                Obstacle obstacle = null;
                if (occupant.TryGetComponent(out obstacle))
                {
                    DebugLogger.Log(this, "Obstacle found!");
                    obstacle.Interact(this);
                    return;
                }

                DebugLogger.Log(this, "Failed interaction because " + occupant + " was in the way.");
                occupant.FailedInteraction();
            }

            if (occupant == null && cell.GroundType.Equals(GroundType.FallowSoil))
            {
                GameManager.Instance.GridManager.ChangeTile(cell.GridPosition, GroundType.ArableSoil);
            }
        }

        public override void UpdateSpriteResolvers(SpriteResolver[] resolvers)
        {
            foreach(SpriteResolver resolver in resolvers)
            {
                resolver.SetCategoryAndLabel("tools", "prepare");
            }
        }

        public override void FailedInteraction()
        {
            return;
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
