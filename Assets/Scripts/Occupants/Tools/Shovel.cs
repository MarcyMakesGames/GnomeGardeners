using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
namespace GnomeGardeners
{
	public class Shovel : Tool
	{
		private Seed seed;

        #region Unity Methods



        #endregion

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
                Seedbag seedbag = null;
                if (seed == null && occupant.TryGetComponent(out seedbag))
                {
                    DebugLogger.Log(this, "Seed taken.");
                    seed = seedbag.GetSunflowerSeed();
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
                    var seedObject = GameManager.Instance.PoolController.GetObjectFromPool(cell.transform.position, Quaternion.identity, seed.plantKey);
                    seed = null;
                    seedObject.GetComponent<Plant>().PlantSeed(cell);
                    GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_spade_digging, audioSource);
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

        public override void FailedInteraction()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
