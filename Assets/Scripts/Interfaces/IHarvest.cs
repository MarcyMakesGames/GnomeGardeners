using UnityEngine;

public interface IHarvest // : IInteractable, IHeldItem
{
    int PointValue { get; set; }
    void Deliver();
}
