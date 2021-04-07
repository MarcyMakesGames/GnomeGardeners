using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string Name { get;}

    /// <summary>
    /// Interact on this object must be done through some kind of tool or handle a null tool circumstance.
    /// </summary>
    void Interact(ITool tool = null);
}
