using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
	public class Shovel : Tool
	{
		private IHoldable holdable;

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
            var seed = (Plant)holdable;

            var occupant = cell.Occupant;
            if (occupant != null)
            {
                Seedbag seedbag = null;
                if (seed == null && occupant.TryGetComponent(out seedbag))
                {
                    DebugLogger.Log(this, "Seed taken.");
                    holdable = seedbag.GetRandomDispensable();
                }
                else if (seed != null && occupant.TryGetComponent(out seedbag))
                {
                    DebugLogger.Log(this, "Seed discarded.");
                    holdable = null;
                }
            }
            else if (seed != null && occupant == null)
            {
                DebugLogger.Log(this, "Seed in hand and no occupant found!");
                if (cell.GroundType == GroundType.ArableSoil)
                {
                    DebugLogger.Log(this, "ArableSoil found!");
                    var seedObject = GameObject.Instantiate(seed.gameObject, cell.transform);
                    seedObject.GetComponent<Plant>().PlantSeed(cell);
                    holdable = null;
                    GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_spade_digging, audioSource);
                }
            }
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
