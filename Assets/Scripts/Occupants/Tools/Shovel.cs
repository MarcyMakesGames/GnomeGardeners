using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
namespace GnomeGardeners
{
	public class Shovel : Tool
	{
		private Seed seed;
        private bool isPlanting = false;

        protected override void Update()
        {
            if (seed != null && popUp == null)
            {
                GetPopUp(seed.PopUpKey);
                SetPopUpLifetime(0f, false);
            }

            base.Update();
        }

        #region Public Methods

        public override void Interact(Tool tool)
        {
            throw new System.NotImplementedException();

        }

        public override void UseTool(GridCell cell)
        {
            isPlanting = false;
            DebugLogger.Log(this, "Executing.");

            var occupant = cell.Occupant;
            if (occupant != null)
            {
                Seedbag seedbag = null;
                if (seed == null && occupant.TryGetComponent(out seedbag))
                {
                    DebugLogger.Log(this, "Seed taken.");
                    seed = seedbag.GetSeed();
                    return;
                }
                else if (seed != null && occupant.TryGetComponent(out seedbag))
                {
                    DebugLogger.Log(this, "Seed discarded.");
                    seed = null;
                    return;
                }

                occupant.FailedInteraction();
            }
            else if (seed != null && occupant == null)
            {
                DebugLogger.Log(this, "Seed in hand and no occupant found!");
                if (cell.GroundType == GroundType.ArableSoil)
                {
                    DebugLogger.Log(this, "ArableSoil found!");
                    var seedObject = GameManager.Instance.PoolController.GetObjectFromPool(cell.transform.position, Quaternion.Euler(-20f, 0f, 0f), seed.plantKey);
                    seed = null;
                    seedObject.GetComponent<Plant>().PlantSeed(cell);
                    GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_spade_digging, audioSource);
                    isPlanting = true;
                }
            }
        }

        public override void UpdateSpriteResolvers(SpriteResolver[] resolvers)
        {
            foreach (SpriteResolver resolver in resolvers)
            {
                resolver.SetCategoryAndLabel("tools", "seed");
            }
        }

        public override void UpdateItemRenderers(SpriteRenderer[] renderers)
        {
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.sprite = seed?.Sprite;
            }
        }

        public override void FailedInteraction()
        {
            throw new System.NotImplementedException();
        }

        public override void PlayCorrespondingAnimation(Animator animator, string prefix)
        {
            base.PlayCorrespondingAnimation(animator, prefix);
            if(isPlanting)
                animator.Play(prefix + "_seed");
        }

        #endregion
    }
}
