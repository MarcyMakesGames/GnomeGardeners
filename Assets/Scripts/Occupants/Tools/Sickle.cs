using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

namespace GnomeGardeners
{
    public class Sickle : Tool
    {
        private Harvest harvest;
        private Fertilizer fertilizer;


        #region Public Methods

        public override void Interact(Tool tool)
        {
            throw new System.NotImplementedException();

        }

        public override void UseTool(GridCell cell)
        {
            DebugLogger.Log(this, "Executing.");
            var occupant = cell.Occupant;
            if (occupant != null)
            {
                DebugLogger.Log(this, "Occupant found!");
                Plant plant;
                if (occupant.TryGetComponent(out plant) && fertilizer != null)
                {
                    DebugLogger.Log(this, "Plant found while carrying Fertilizer!");
                    plant.FulfillCurrentNeed(NeedType.Fertilizer);
                    fertilizer = null;
                    return;
                }
                else if (occupant.TryGetComponent(out plant) && harvest == null)
                {
                    DebugLogger.Log(this, "Harvesting plant!");
                    harvest = plant.HarvestPlant();
                    return;
                }

                Basket basket;
                if (occupant.TryGetComponent(out basket) && harvest != null)
                {
                    DebugLogger.Log(this, "Scoring Area found!");
                    basket.DeliverHarvest(harvest.points);
                    harvest = null;
                    return;
                }

                Compost compost;
                if (occupant.TryGetComponent(out compost))
                {
                    DebugLogger.Log(this, "Compost found!"); 
                    if (fertilizer != null)
                    {
                        DebugLogger.Log(this, "Discarding fertilizer.");
                        fertilizer = null;
                    }

                    if (fertilizer == null)
                    {
                        DebugLogger.Log(this, "Taking fertilizer.");
                        fertilizer = compost.DispenseItem();
                    }
                    
                    if(harvest != null)
                    {
                        DebugLogger.Log(this, "Discarding harvest");
                        compost.AddScore(harvest.points);
                        harvest = null;
                    }

                    return;
                }

                occupant.FailedInteraction();
            }
        }
        public override void UpdateSpriteResolvers(SpriteResolver[] resolvers)
        {
            foreach (SpriteResolver resolver in resolvers)
            {
                resolver.SetCategoryAndLabel("tools", "harvest");
            }
        }

        public override void FailedInteraction()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
