using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCommand : ICommand
{
    private bool debug = true;

    public void Execute(GridCell cell, Tool tool, GnomeController gnome)
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
                    if (!plant.CurrentStage.isHarvestable) { return; }
                    tool.heldItem = holdable;
                    plant.HarvestPlant(cell);
                    gnome.SetItemSprite(plant.SpriteInHand);
                }

                var scoringArea = associatedObject.GetComponent<ScoringArea>();
                if (scoringArea != null && tool.heldItem != null)
                {
                    scoringArea.Interact(tool);
                    tool.heldItem = null;
                    gnome.RemoveItemSprite();
                }

                var compost = associatedObject.GetComponent<Compost>();
                if (compost != null)
                {
                    Log("Compost found!");
                    compost.Interact(tool);
                    var fertilizer = tool.heldItem;
                    gnome.SetItemSprite(fertilizer.SpriteInHand);
                }
            }
            else
            {
                LogWarning("Occupant does not have an associated object.");
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
