using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCommand : ICommand
{
    public void Execute(GridCell cell, Tool tool)
    {
        Debug.Log("Executing Harvest Command.");

        var plant = cell.Occupant.AssociatedObject.GetComponent<Plant>();

        if ( plant != null){
            plant.HarvestPlant();
            tool.heldItem = plant;
        }

        var scoringArea = cell.Occupant.AssociatedObject.GetComponent<ScoringArea>();

        if(scoringArea != null)
        {
            scoringArea.Interact(tool);
        }
    }
}
