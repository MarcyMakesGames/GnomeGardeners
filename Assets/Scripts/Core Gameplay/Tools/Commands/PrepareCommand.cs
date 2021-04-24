using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareCommand : ICommand
{
    public void Execute(GridCell cell, Tool tool)
    {
        Debug.Log("Executing Prepare Command.");

        /*if (cell.CellOccupant.Equals(Obstacle)){
         * cell.RemoveCellOccupant();
        *
        */


        if (cell.GroundType.Equals(GroundType.FallowSoil)){
            GameManager.Instance.GridManager.ChangeTile(cell.GridPosition, GroundType.ArableSoil);
        }
    }
}
