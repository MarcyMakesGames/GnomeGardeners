using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareCommand : ICommand
{
    public void Execute(GridCell cell, Tool tool)
    {
        /* if in front of fallow ground
         *  make ground arable
         *  
         * if in front of obstacle
         *  remove obstacle
         */

        /*if (cell.CellOccupant.Equals(Obstacle)){
         * cell.RemoveCellOccupant();
        *
        */
        if (cell.GroundType.Equals(GroundType.FallowSoil)){
            cell.GroundType = GroundType.ArableSoil;
        }
    }
}
