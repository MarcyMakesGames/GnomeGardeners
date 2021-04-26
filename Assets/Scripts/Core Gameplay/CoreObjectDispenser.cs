using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreObjectDispenser : MonoBehaviour, IInteractable
{
    public GameObject dispensable;
    public GameObject AssociatedObject { get => gameObject; }

    #region Public Methods
    private void DispenseItem(Tool tool)
    {
        var dispensedItem = Instantiate(dispensable, transform);
        tool.heldItem = dispensedItem.GetComponent<IHoldable>();
        dispensedItem.gameObject.SetActive(false);
    }

    private void DispenseItem(Vector2Int dropLocation)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Tool tool = null)
    {
        if (tool != null)
            DispenseItem(tool);
        else
            return; // todo: implement drop on floor

    }
    #endregion
}
