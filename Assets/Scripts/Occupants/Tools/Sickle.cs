using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Sickle : Tool
    {
        private IHoldable holdable;

        #region Unity Methods



        #endregion

        #region Public Methods

        public override void Interact(Tool tool)
        {
            throw new System.NotImplementedException();

        }

        public override void UseTool(GridCell cell, GnomeController gnome)
        {
            DebugLogger.Log(this, "Executing.");
            var occupant = cell.Occupant;
            if (occupant != null)
            {
                DebugLogger.Log(this, "Occupant found!");
                var holdable = occupant.GetComponent<IHoldable>();
                Plant plant = null;
                if (occupant.TryGetComponent(out plant))
                {
                    DebugLogger.Log(this, "Plant found while carrying Fertilizer!");
                    plant.FulfillCurrentNeed(NeedType.Fertilizer);
                    holdable = null;
                    gnome.RemoveItemSprite();
                }
                else if (occupant.TryGetComponent(out plant) && holdable == null && holdable != null)
                {
                    DebugLogger.Log(this, "Harvesting plant!");
                    plant.HarvestPlant();
                    holdable = holdable;
                    plant.transform.parent = gnome.transform;
                    gnome.SetItemSprite(plant.SpriteInHand);
                }

                Basket basket = null;
                if (occupant.TryGetComponent(out basket) && holdable != null)
                {
                    DebugLogger.Log(this, "Scoring Area found!");
                    var harvest = (Plant)holdable;
                    basket.DeliverHarvest(harvest.CurrentStage.pointValue);
                    GameObject.Destroy(harvest.gameObject);
                    holdable = null;
                    gnome.RemoveItemSprite();
                }

                Compost compost = null;
                if (occupant.TryGetComponent(out compost))
                {
                    DebugLogger.Log(this, "Compost found!");
                    if (holdable == null)
                    {
                        DebugLogger.Log(this, "Taking fertilizer.");
                        compost.Interact(this);
                        var fertilizer = holdable;
                        gnome.SetItemSprite(fertilizer.SpriteInHand);
                    }
                    else
                    {
                        DebugLogger.Log(this, "Discarding fertilizer.");
                        holdable = null;
                        gnome.RemoveItemSprite();
                    }
                }
            }
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
