using UnityEngine;

public class CarryingTool : CoreTool, ITool
{
    private GameObject heldItem = null;
    [SerializeField] protected bool is2D;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public new void Interact(ITool tool = null)
    {
        // todo: gets equipped
        base.Interact(tool);
    }

    public new void UseTool(Vector3 origin, Vector3 direction, float distance)
    {
        if (is2D)
        {
            if (heldItem != null)
            {
                GridManager gridManager = FindObjectOfType<GridManager>();

                heldItem.GetComponent<IHeldItem>().DropItem(origin, direction);
                gridManager.ChangeTileOccupant(gridManager.GetClosestGrid(origin + direction), heldItem.GetComponent<IInteractable>());
            }
        }
        else
        {
            base.UseTool(origin, direction, distance);
            if (heldItem != null)
            {
                heldItem.GetComponent<IHeldItem>().DropItem(origin, direction);
                heldItem = null;
            }

            if (lastHitTransform.GetComponent<IHeldItem>() != null)
            {
                heldItem = lastHitTransform.gameObject;
            }
        }
    }

    public new void DropItem(Vector3 position, Vector3 direction)
    {
        // todo: drop tool
        base.DropItem(position, direction);
        rb.AddForce(direction * dropStrength);
        Debug.Log("Dropped carrying tool.");
    }
}
