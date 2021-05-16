using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{

    public class HarvestCommand : ICommand
    {
        private bool debug = false;

        public void Execute(GridCell cell, Tool tool, GnomeController gnome)
        {
            DebugLogger.Log(this, "Executing.");
            var occupant = cell.Occupant;
            if(occupant != null)
            {
                DebugLogger.Log(this, "Occupant found!");
                var associatedObject = occupant.AssociatedObject;
                if(associatedObject != null)
                {
                    var plant = associatedObject.GetComponent<Plant>();
                    var holdable = associatedObject.GetComponent<IHoldable>();
                    if(plant != null && tool.heldItem != null)
                    {
                        DebugLogger.Log(this, "Plant found while carrying Fertilizer!");
                        var fertilizer = (Fertilizer)tool.heldItem;
                        plant.AddToNeedValue(NeedType.Fertilizer, fertilizer.Strength);
                        tool.heldItem = null;
                        gnome.RemoveItemSprite();
                    }
                    else if (plant != null && tool.heldItem == null && holdable != null)
                    {
                        DebugLogger.Log(this, "Harvesting plant!");
                        if (!plant.CurrentStage.isHarvestable) { return; }
                        tool.heldItem = holdable;
                        plant.transform.parent = gnome.transform;
                        plant.HarvestPlant(cell);
                        gnome.SetItemSprite(plant.SpriteInHand);
                    }

                    var scoringArea = associatedObject.GetComponent<ScoringArea>();
                    if (scoringArea != null && tool.heldItem != null)
                    {
                        DebugLogger.Log(this, "Scoring Area found!");
                        scoringArea.Interact(tool);
                        var harvest = (Plant)tool.heldItem;
                        GameObject.Destroy(harvest.gameObject);
                        tool.heldItem = null;
                        gnome.RemoveItemSprite();
                    }

                    var compost = associatedObject.GetComponent<Compost>();
                    if (compost != null)
                    {
                        DebugLogger.Log(this, "Compost found!");
                        if(tool.heldItem == null)
                        {
                            DebugLogger.Log(this, "Taking fertilizer.");
                            compost.Interact(tool);
                            var fertilizer = tool.heldItem;
                            gnome.SetItemSprite(fertilizer.SpriteInHand);
                        }
                        else
                        {
                            DebugLogger.Log(this, "Discarding fertilizer.");
                            tool.heldItem = null;
                            gnome.RemoveItemSprite();
                        }
                    }
                }
                else
                {
                    DebugLogger.LogWarning(this, "Occupant does not have an associated object.");
                }
            }
        
        }
    }
}
