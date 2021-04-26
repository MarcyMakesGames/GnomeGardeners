using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCommand : ICommand
{
    public void Execute(GridCell cell, Tool tool)
    {
        Debug.Log("Executing Seed Command.");
        var seed = (Plant)tool.heldItem;

        var seedBag = cell.Occupant.AssociatedObject.GetComponent<CoreObjectDispenser>();
        if (seed == null && seedBag != null)
        {
            seedBag.Interact(tool);
        }
        else if (seed.CurrentStage.specifier == PlantStage.Seed && cell.GroundType == GroundType.ArableSoil)
        {
            seed.PlantSeed(cell);
        }
    }
}
