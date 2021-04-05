using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour, IHarvest
{
    public int PointValue { get; set; }

    public void Deliver()
    {
        // deliver harvest to truck; increase score by pointValue
        throw new System.NotImplementedException();
    }

    public void DropItem(Vector3 position, Vector3 direction)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(ITool tool = null)
    {
        throw new System.NotImplementedException();
    }
}
