using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreObjectDispenser : MonoBehaviour, IObjectDispenser
{
    [SerializeField] List<GameObject> dispensables;

    #region Public Methods
    public void DispenseItem(ITool tool, string itemName)
    {
        CarryingTool toolUsed = (CarryingTool)tool;

        if (toolUsed == null || toolUsed.Type != ToolType.Carrying)
            return;

        foreach(GameObject item in dispensables)
        {

            if (itemName == item.name)
            {
                GameObject newPlant = Instantiate(item, transform.position, transform.rotation);
                toolUsed.HeldItem = newPlant;
                newPlant.SetActive(false);
            }
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
        DispenseItem(tool, "Plant");
    }
    #endregion
}
