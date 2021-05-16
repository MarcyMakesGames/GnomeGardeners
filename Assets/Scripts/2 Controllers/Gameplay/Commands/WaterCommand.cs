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
            if(occupant != null)
            {
                Plant plant = null;
                if (occupant.TryGetComponent(out plant))
                {
                    plant.AddToNeedValue(NeedType.Water, tool.waterAmount);
                }

                Insect insect = null;
                if(occupant.TryGetComponent(out insect) && occupant.TryGetComponent(out insect))
                {
                    insect.IncrementShooedCount();
                }
            }
        }
    }
}
