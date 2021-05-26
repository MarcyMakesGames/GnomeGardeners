using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.PlayerLoop;

namespace GnomeGardeners
{
    public class Sickle : Tool
    {
        private Harvest harvest;
        private bool isHarvesting = false;

        private void Update()
        {
            if(harvest != null && popUp == null)
                GetPopUp(harvest.PopUpKey);
        }

        #region Public Methods

        public override void Interact(Tool tool)
        {
            throw new System.NotImplementedException();

        }

        public override void UseTool(GridCell cell)
        {
            isHarvesting = false;
            DebugLogger.Log(this, "Executing.");
            var occupant = cell.Occupant;
            if (occupant != null)
            {
                DebugLogger.Log(this, "Occupant found!");
                Plant plant;
                if (occupant.TryGetComponent(out plant) && harvest == null)
                {
                    DebugLogger.Log(this, "Harvesting plant!");
                    harvest = plant.HarvestPlant();
                    isHarvesting = true;
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
                    if (harvest != null)
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

        public override void UpdateItemRenderers(SpriteRenderer[] renderers)
        {
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.sprite = harvest?.Sprite;
            }
        }

        public override void FailedInteraction()
        {
            throw new System.NotImplementedException();
        }

        public override void PlayCorrespondingAnimation(Animator animator, string prefix)
        {
            base.PlayCorrespondingAnimation(animator, prefix);
            if(isHarvesting)
                animator.Play(prefix + "_harvest");
        }

        #endregion
    }
}
