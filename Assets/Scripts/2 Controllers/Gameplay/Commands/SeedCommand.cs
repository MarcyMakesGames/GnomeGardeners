using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class SeedCommand : ICommand
    {
        private bool debug = false;

        public void Execute(GridCell cell, Tool tool, GnomeController gnome)
        {
            DebugLogger.Log(this, "Executing.");
            var seed = (Plant)tool.heldItem;

            var occupant = cell.Occupant;
            if (occupant != null)
            {
                Seedbag seedbag = null;
                if (seed == null && occupant.TryGetComponent(out seedbag))
                {
                    DebugLogger.Log(this, "Seed taken.");
                    seedbag.Interact(tool);
                    gnome.SetItemSpriteToSeed(); 
                }
                else if (seed != null && occupant.TryGetComponent(out seedbag))
                {
                    DebugLogger.Log(this, "Seed discarded.");
                    tool.heldItem = null;
                    gnome.RemoveItemSprite();
                }
            }
            else if(seed != null && occupant == null)
            {
                DebugLogger.Log(this, "Seed in hand and no occupant found!");
                if (cell.GroundType == GroundType.ArableSoil)
                {
                    DebugLogger.Log(this, "ArableSoil found!");
                    var seedObject = GameObject.Instantiate(seed.gameObject, cell.transform);
                    seedObject.GetComponent<Plant>().PlantSeed(cell);
                    gnome.RemoveItemSprite();
                    tool.heldItem = null;
                    GameManager.Instance.AudioManager.PlaySound(SoundType.sfx_spade_digging, tool.AudioSource);
                }
            }
        }
    }
}
