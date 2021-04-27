using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareCommand : ICommand
{
    private bool debug = false;

    public void Execute(GridCell cell, Tool tool)
    {
        Log("Executing");

        /*if (cell.CellOccupant.Equals(Obstacle)){
         * cell.RemoveCellOccupant();
        *
        */


        if (cell.GroundType.Equals(GroundType.FallowSoil)){
            GameManager.Instance.GridManager.ChangeTile(cell.GridPosition, GroundType.ArableSoil);
        }
    }

    private void Log(string msg)
    {
        if (debug)
            Debug.Log("[PrepareCommand]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (debug)
            Debug.LogWarning("[PrepareCommand]: " + msg);
    }
}
