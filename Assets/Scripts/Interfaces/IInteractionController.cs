using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionController
{
    GameObject Interact(Vector2 origin, Vector2 destination, Tool tool = null);
}
