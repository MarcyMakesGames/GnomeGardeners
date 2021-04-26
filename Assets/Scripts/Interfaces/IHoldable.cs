using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldable
{
    void Hold();
    void Drop(Vector2 position);
    bool IsBeingCarried { get; set; }
}
