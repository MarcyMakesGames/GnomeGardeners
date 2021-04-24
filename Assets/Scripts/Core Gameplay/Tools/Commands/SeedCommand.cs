using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCommand : ICommand
{
    private Plant seed = null;

    public void Execute(GridCell cell, Tool tool)
    {
        Debug.Log("Executing Seed Command.");

        var seedBag = cell.Occupant.AssociatedObject.GetComponent<CoreObjectDispenser>();
        if (seed == null && seedBag != null)
        {
            seedBag.DispenseItem(tool, "Plant");
        }
        else if (seed.CurrentStage.specifier == PlantStage.Seed && cell.GroundType == GroundType.ArableSoil)
        {
            seed.PlantSeed(cell);
        }


    }
}
