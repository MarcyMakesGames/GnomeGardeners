using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{

    public class PrepareCommand : ICommand
    {
        private bool debug = false;

        public void Execute(GridCell cell, Tool tool, GnomeController gnome)
        {
            DebugLogger.Log(this, "Executing");
            var occupant = cell.Occupant;
            if (occupant != null)
            {
                DebugLogger.Log(this, "Occupant found!");
                var associatedObject = occupant.AssociatedObject;
                var obstacle = associatedObject.GetComponent<Obstacle>();
                if(obstacle != null )
                {
                    DebugLogger.Log(this, "Obstacle found!");
                    obstacle.Interact(tool);
                }
            }

            if (occupant == null && cell.GroundType.Equals(GroundType.FallowSoil))
            {
                GameManager.Instance.GridManager.ChangeTile(cell.GridPosition, GroundType.ArableSoil);
            }
        }
    }
}
