using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCommand : ICommand
{
    private bool debug = false;

    public void Execute(GridCell cell, Tool tool)
    {
        Log("Executing.");
        var occupant = cell.Occupant;
        if(occupant != null)
        {
            var associatedObject = occupant.AssociatedObject;
            if(associatedObject != null)
            {
                var plant = associatedObject.GetComponent<Plant>();
                var holdable = associatedObject.GetComponent<IHoldable>();
                if (plant != null && tool.heldItem == null && holdable != null)
                {
                    tool.heldItem = holdable;
                    plant.HarvestPlant(cell);
                }

                var scoringArea = associatedObject.GetComponent<ScoringArea>();
                if (scoringArea != null && tool.heldItem != null)
                {
                    scoringArea.Interact(tool);
                    tool.heldItem = null;
                }
            }
        }
        
    }

    private void Log(string msg)
    {
        if (debug)
            Debug.Log("[HarvestCommand]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (debug)
            Debug.LogWarning("[HarvestCommand]: " + msg);
    }
}
