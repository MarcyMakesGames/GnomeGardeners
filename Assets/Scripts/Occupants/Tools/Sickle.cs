using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Sickle : Tool
    {
        private Item item;

        #region Unity Methods



        #endregion

        #region Public Methods

        public override void Interact(Tool tool)
        {
            throw new System.NotImplementedException();

        }

        public override void UseTool(GridCell cell, Gnome gnome)
        {
            DebugLogger.Log(this, "Executing.");
            var occupant = cell.Occupant;
            if (occupant != null)
            {
                DebugLogger.Log(this, "Occupant found!");
                var holdable = occupant.GetComponent<IHoldable>();
                Plant plant;
                if (occupant.TryGetComponent(out plant))
                {
                    DebugLogger.Log(this, "Plant found while carrying Fertilizer!");
                    plant.FulfillCurrentNeed(NeedType.Fertilizer);
                    holdable = null;
                }
                else if (occupant.TryGetComponent(out plant) && holdable == null && holdable != null)
                {
                    DebugLogger.Log(this, "Harvesting plant!");
                    plant.HarvestPlant();
                    plant.transform.parent = gnome.transform;
                }

                Basket basket;
                if (occupant.TryGetComponent(out basket) && holdable != null)
                {
                    DebugLogger.Log(this, "Scoring Area found!");
                    var harvest = (Plant)holdable;
                    basket.DeliverHarvest(harvest.CurrentStage.pointValue);
                    GameObject.Destroy(harvest.gameObject);
                    holdable = null;
                }

                Compost compost;
                if (occupant.TryGetComponent(out compost))
                {
                    DebugLogger.Log(this, "Compost found!");
                    if (holdable == null)
                    {
                        DebugLogger.Log(this, "Taking fertilizer.");
                        item = compost.DispenseItem();
                    }
                    else
                    {
                        DebugLogger.Log(this, "Discarding fertilizer.");
                        item = null;
                    }
                }
            }
        }

        #endregion

        #region Private Methods



        #endregion

    }
}
