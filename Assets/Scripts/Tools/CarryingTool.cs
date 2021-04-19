using UnityEngine;

public class CarryingTool : CoreTool, ITool
{
    private GameObject heldItem = null;
    [SerializeField] private float dropRange = 1f;
    public GameObject HeldItem { get => heldItem; set => heldItem = value; }

    public new void Interact(ITool tool = null)
    {
        // todo: gets equipped
        base.Interact(tool);

    }

    public new void UseTool(Vector3 origin, Vector3 direction, float distance)
    {
        lastHitTransform = null;
        base.UseTool(origin, direction, distance);

        if (heldItem != null)
        {
            if (heldItem.GetComponent<Harvest>() != null)
            {
                if(heldItem.GetComponent<Harvest>().Deliver(origin, direction, distance))
                {
                    heldItem = null;
                    return;
                }
            }

                heldItem.GetComponent<IHoldable>().Drop(origin + direction * dropRange);
                heldItem = null;
                

        }

            if (lastHitTransform == null)
                return;

            if (lastHitTransform.GetComponent<IHoldable>() != null)
            {
                heldItem = lastHitTransform.gameObject;
            }
            
        if(lastHitTransform.GetComponent<IObjectDispenser>() != null)
        {
            lastHitTransform.GetComponent<IObjectDispenser>().DispenseItem(this, "Plant");
        }
    }

    public new void Drop(Vector2 position)
    {
        // todo: drop tool
        base.Drop(position);
        Debug.Log("Dropped carrying tool.");
    }
}
