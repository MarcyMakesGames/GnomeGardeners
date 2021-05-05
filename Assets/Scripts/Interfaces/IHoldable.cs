using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldable
{
    bool IsBeingCarried { get; set; }
    Sprite SpriteInHand { get; set; }
}
