using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{

    public class WaterCommand : ICommand
    {
        private bool debug = false;
        public void Execute(GridCell cell, Tool tool, GnomeController gnome)
        {
            DebugLogger.Log(this, "Executing");
            var occupant = cell.Occupant;
            if(occupant == null)
            {
                return;
            }

            var plant = cell.Occupant.AssociatedObject.GetComponent<Plant>();
            if (plant != null)
            {
                plant.AddToNeedValue(NeedType.Water, tool.waterAmount);
            }

            var insect = cell.Occupant.AssociatedObject.GetComponent<Insect>();
            if(insect != null)
            {
                insect.IncrementShooedCount();
            }
        }
    }
}
