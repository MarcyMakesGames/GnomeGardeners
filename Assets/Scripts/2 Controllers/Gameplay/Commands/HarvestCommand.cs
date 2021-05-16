using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class HarvestCommand : ICommand
    {
        public void Execute(GridCell cell, Tool tool, GnomeController gnome)
        {
            DebugLogger.Log(this, "Executing.");
            var occupant = cell.Occupant;
            if (occupant != null)
            {
                DebugLogger.Log(this, "Occupant found!");
                var holdable = occupant.GetComponent<IHoldable>();
                Plant plant = null;
                if (occupant.TryGetComponent(out plant) && tool.heldItem != null)
                {
                    DebugLogger.Log(this, "Plant found while carrying Fertilizer!");
                    var fertilizer = (Fertilizer)tool.heldItem;
                    plant.AddToNeedValue(NeedType.Fertilizer, fertilizer.Strength);
                    tool.heldItem = null;
                    gnome.RemoveItemSprite();
                }
                else if (occupant.TryGetComponent(out plant) && tool.heldItem == null && holdable != null)
                {
                    DebugLogger.Log(this, "Harvesting plant!");
                    if (!plant.CurrentStage.isHarvestable) { return; }
                    tool.heldItem = holdable;
                    plant.transform.parent = gnome.transform;
                    plant.HarvestPlant(cell);
                    gnome.SetItemSprite(plant.SpriteInHand);
                }

                Basket basket = null;
                if (occupant.TryGetComponent(out basket) && tool.heldItem != null)
                {
                    DebugLogger.Log(this, "Scoring Area found!");
                    basket.Interact(tool);
                    var harvest = (Plant)tool.heldItem;
                    GameObject.Destroy(harvest.gameObject);
                    tool.heldItem = null;
                    gnome.RemoveItemSprite();
                }

                Compost compost = null;
                if (occupant.TryGetComponent(out compost))
                {
                    DebugLogger.Log(this, "Compost found!");
                    if (tool.heldItem == null)
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
        }
    }
}
