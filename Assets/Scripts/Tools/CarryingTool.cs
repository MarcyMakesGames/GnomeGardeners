using UnityEngine;

public class CarryingTool : CoreTool, ITool
{

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public new void Interact(ITool tool = null)
    {
        // todo: gets equipped
        base.Interact();
        Debug.Log("Equipped carrying tool.");
    }

    public new void UseTool(Vector2 usePosition, Vector2 useDirection, float useRange)
    {
        // todo: pick up object IHeldItem in front of gnome
    }
    public new void DropItem(Vector3 position, Vector3 direction)
    {
        // todo: drop tool
        base.DropItem(position, direction);
        rb.AddForce(direction * dropStrength);
        Debug.Log("Dropped carrying tool.");
    }
}
