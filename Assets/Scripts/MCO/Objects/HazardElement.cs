using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HazardElement
{
    private int uniqueID;
    private Direction direction;

    public int UniqueID { get => uniqueID; }
    public Direction Direction { get => direction; }

    public virtual void SpawnElement() { }
}