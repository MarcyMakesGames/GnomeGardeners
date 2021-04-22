using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCommand : ICommand
{
    public void Execute(GridCell cell, Tool tool)
    {
        var plant = cell.Occupant.GameObject.GetComponent<Plant>();

        if ( plant != null){
            plant.HarvestPlant();
            tool.heldItem = plant;
        }

        var scoringArea = cell.Occupant.GameObject.GetComponent<ScoringArea>();

        if(scoringArea != null)
        {
            scoringArea.Interact(tool);
        }
    }
}
