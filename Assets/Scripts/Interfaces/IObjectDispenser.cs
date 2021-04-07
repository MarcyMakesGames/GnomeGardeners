using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectDispenser : IInteractable
{
    void DispenseItem(ITool tool, string itemName);

    void DispenseItem(Vector3 dropLocation, string itemName);
}
