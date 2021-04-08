using UnityEngine;

public class CarryingTool : CoreTool, ITool
{
    private GameObject heldItem = null;
    [SerializeField] protected bool is2D;
    [SerializeField] private float dropRange = 3f;

    public GameObject HeldItem { get => heldItem; set => heldItem = value; }

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
        lastHitTransform = null;

        if (is2D)
        {
            base.UseTool(origin, direction, 1);

            if(lastHitTransform == null && heldItem != null)
            {
                GridManager gridManager = FindObjectOfType<GridManager>();

                heldItem.GetComponent<IHeldItem>().DropItem(origin, direction);
                gridManager.ChangeTileOccupant(gridManager.GetClosestGrid(origin + direction), heldItem.GetComponent<IInteractable>());

                heldItem = null;
                return;
            }

            if(lastHitTransform != null && heldItem == null)
            {
                if (lastHitTransform.GetComponent<IObjectDispenser>() != null)
                    lastHitTransform.GetComponent<IObjectDispenser>().DispenseItem(this, "Plant");

                if (lastHitTransform.GetComponent<Plant>() != null || lastHitTransform.GetComponent<CoreTool>() != null || lastHitTransform.GetComponent<Harvest>() != null)
                {
                    heldItem = lastHitTransform.gameObject;
                    lastHitTransform.gameObject.SetActive(false);

                    lastHitTransform = null;
                }

                return;

                //Some kind of interaction
            }

            if(lastHitTransform != null && heldItem != null)
            {
                if (lastHitTransform.GetComponent<Truck>() != null && heldItem.GetComponent<IHarvest>() != null)
                {
                    heldItem.GetComponent<IHarvest>().Deliver(origin, direction, distance);
                    Destroy(heldItem);
                }

                if (lastHitTransform.GetComponent<Plant>() != null || lastHitTransform.GetComponent<CoreTool>() != null || lastHitTransform.GetComponent<Harvest>() != null)
                {
                    GridManager gridManager = FindObjectOfType<GridManager>();

                    heldItem.GetComponent<IHeldItem>().DropItem(origin, direction);
                    gridManager.ChangeTileOccupant(gridManager.GetClosestGrid(origin + direction), heldItem.GetComponent<IInteractable>());

                    heldItem = lastHitTransform.gameObject;
                    lastHitTransform.gameObject.SetActive(false);

                    lastHitTransform = null;
                }
            }
        }

        else
        {
            base.UseTool(origin, direction, distance);
            if (heldItem != null)
            {
                if (heldItem.GetComponent<IHarvest>() != null)
                {
                    if(heldItem.GetComponent<IHarvest>().Deliver(origin, direction, distance))
                    {
                        heldItem = null;
                        return;
                    }
                }

                heldItem.GetComponent<IHeldItem>().DropItem(origin + direction * dropRange, direction);
                heldItem = null;
                

            }

            if (lastHitTransform.GetComponent<IHeldItem>() != null)
            {
                heldItem = lastHitTransform.gameObject;
            }
            
            if(lastHitTransform.GetComponent<IObjectDispenser>() != null)
            {
                lastHitTransform.GetComponent<IObjectDispenser>().DispenseItem(this, "Plant");
            }
        }
    }

    public new void DropItem(Vector3 position, Vector3 direction)
    {
        // todo: drop tool
        base.DropItem(position, direction);
        heldItem = null;
        Debug.Log("Dropped carrying tool.");
    }
}
