using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCommand : ICommand
{
    private bool debug = false;

    public void Execute(GridCell cell, Tool tool, GnomeController gnome)
    {
        Log("Executing.");
        var seed = (Plant)tool.heldItem;

        var occupant = cell.Occupant;
        if (occupant != null)
        {
            Log("Occupant found!");
            var associatedObject = occupant.AssociatedObject;
            var seedBag = associatedObject.GetComponent<CoreObjectDispenser>();
            if (seed == null && seedBag != null)
            {
                Log("Seedbag found!");
                seedBag.Interact(tool);
                gnome.SetItemSpriteToSeed(); 
            }
        }
        else if(seed != null && occupant == null)
        {
            Log("Seed in hand and no occupant found!");
            if (seed.CurrentStage.isPlantable && cell.GroundType == GroundType.ArableSoil)
            {
                Log("ArableSoil found!");
                seed.PlantSeed(cell);
                gnome.RemoveItemSprite();
                tool.heldItem = null;
            }
        }
    }

    private void Log(string msg)
    {
        if(debug)
            Debug.Log("[SeedCommand]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if(debug)
            Debug.LogWarning("[SeedCommand]: " + msg);
    }
}
