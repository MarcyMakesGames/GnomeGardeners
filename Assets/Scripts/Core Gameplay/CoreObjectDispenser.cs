using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreObjectDispenser : MonoBehaviour, IInteractable
{
    public GameObject GameObject => gameObject;

    #region Public Methods
    public void DispenseItem(Tool tool, string itemName)
    {
        
    }

    public void DispenseItem(Vector3 dropLocation, string itemName)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Tool tool = null)
    {
        DispenseItem(tool, "Plant");
    }
    #endregion
}
