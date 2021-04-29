using UnityEngine;

public interface IOccupant
{
    public GameObject AssociatedObject { get; }

    public void AssignOccupant();
}
