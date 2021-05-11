using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCommand : ICommand
{
    private bool debug = false;

    public void Execute(GridCell cell, Tool tool, GnomeController gnome)
    {
        Log("Executing.");
        var occupant = cell.Occupant;
        if(occupant != null)
        {
            Log("Occupant found!");
            var associatedObject = occupant.AssociatedObject;
            if(associatedObject != null)
            {
                var plant = associatedObject.GetComponent<Plant>();
                var holdable = associatedObject.GetComponent<IHoldable>();
                if(plant != null && tool.heldItem != null)
                {
                    Log("Plant found while carrying Fertilizer!");
                    var fertilizer = (Fertilizer)tool.heldItem;
                    plant.AddToNeedValue(NeedType.Fertilizer, fertilizer.Strength);
                    tool.heldItem = null;
                    gnome.RemoveItemSprite();
                }
                else if (plant != null && tool.heldItem == null && holdable != null)
                {
                    Log("Harvesting plant!");
                    if (!plant.CurrentStage.isHarvestable) { return; }
                    tool.heldItem = holdable;
                    plant.transform.parent = gnome.transform;
                    plant.HarvestPlant(cell);
                    gnome.SetItemSprite(plant.SpriteInHand);
                }

                var scoringArea = associatedObject.GetComponent<ScoringArea>();
                if (scoringArea != null && tool.heldItem != null)
                {
                    Log("Scoring Area found!");
                    scoringArea.Interact(tool);
                    var harvest = (Plant)tool.heldItem;
                    GameObject.Destroy(harvest.gameObject);
                    tool.heldItem = null;
                    gnome.RemoveItemSprite();
                }

                var compost = associatedObject.GetComponent<Compost>();
                if (compost != null)
                {
                    Log("Compost found!");
                    if(tool.heldItem == null)
                    {
                        Log("Taking fertilizer.");
                        compost.Interact(tool);
                        var fertilizer = tool.heldItem;
                        gnome.SetItemSprite(fertilizer.SpriteInHand);
                    }
                    else
                    {
                        Log("Discarding fertilizer.");
                        tool.heldItem = null;
                        gnome.RemoveItemSprite();
                    }
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
