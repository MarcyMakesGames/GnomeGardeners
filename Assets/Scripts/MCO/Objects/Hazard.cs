using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard
{
    private List<HazardElement> hazardElements;

    public void SpawnHazard() 
    {
        foreach (HazardElement element in hazardElements)
            element.SpawnElement();
    }
}
