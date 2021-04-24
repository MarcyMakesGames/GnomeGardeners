using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreObjectDispenser : MonoBehaviour, IInteractable
{
    public GameObject AssociatedObject { get => gameObject; }

    private void Start()
    {
    }

    #region Public Methods
    public void DispenseItem(Tool tool, string itemName)
    {
        throw new System.NotImplementedException();
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
