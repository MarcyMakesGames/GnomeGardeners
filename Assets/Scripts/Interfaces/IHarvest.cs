using UnityEngine;

public interface IHarvest : IInteractable, IHeldItem
{
    int PointValue { get; set; }
    bool Deliver(Vector3 origin, Vector3 direction, float distance);
}
