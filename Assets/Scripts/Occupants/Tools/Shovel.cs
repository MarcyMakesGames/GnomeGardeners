using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public override void UseTool(GridCell cell, Gnome gnome)
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
                }
                else if (seed != null && occupant.TryGetComponent(out seedbag))
                {
                    DebugLogger.Log(this, "Seed discarded.");
                    seed = null;
                }
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

        #endregion

        #region Private Methods



        #endregion
    }
}
