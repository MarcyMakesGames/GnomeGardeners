using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreObjectDispenser : MonoBehaviour, IObjectDispenser
{
    protected string objectName = "Seed Dispenser";
    List<GameObject> dispensables;

    public string Name => objectName;

    public void DispenseItem(ITool tool, string itemName)
    {
        CarryingTool toolUsed = (CarryingTool)tool;

        if (toolUsed.Type != ToolType.Carrying)
            return;

        foreach(GameObject item in dispensables)
        {
            IInteractable itemInteractable = item.GetComponent<IInteractable>();

            if (itemName == itemInteractable.Name)
                toolUsed.HeldItem = item;
            return;
        }

        Debug.Log("No object with name " + itemName + " found in list.");
    }

    public void DispenseItem(Vector3 dropLocation, string itemName)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(ITool tool = null)
    {
        throw new System.NotImplementedException();
    }
}
