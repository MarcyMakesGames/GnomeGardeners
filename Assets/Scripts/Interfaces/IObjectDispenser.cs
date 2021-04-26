using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectDispenser : IInteractable
{
    void DispenseItem(Tool tool, string itemName);
    void DispenseItem(Vector2Int dropLocation, string itemName);
}
