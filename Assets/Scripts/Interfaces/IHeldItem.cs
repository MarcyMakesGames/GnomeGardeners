using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHeldItem
{
    void DropItem(Vector3 position, Vector3 direction);
}
