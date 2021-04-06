using UnityEngine;

public class DiggingTool : CoreTool, ITool
{
    [SerializeField] protected bool is2D;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public new void Interact(ITool tool = null)
    {
        // todo: gets equipped
        base.Interact();
        Debug.Log("Equipped digging tool.");
    }

    public new void UseTool(Vector3 origin, Vector3 direction, float distance)
    {
        if(is2D)
        {
            GridManager gridManager = FindObjectOfType<GridManager>();
            gridManager.ChangeTile(gridManager.GetClosestGrid(origin + direction), GroundType.Arable);
        }

        else
        {
            base.UseTool(origin, direction, distance);
        }
    }

    public new void DropItem(Vector3 position, Vector3 direction)
    {
        // todo: drop tool
        base.DropItem(position, direction);
        rb.AddForce(direction * dropStrength);
        Debug.Log("Dropped digging tool.");
    }
}
