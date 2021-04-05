using UnityEngine;

public class DiggingTool : CoreTool, ITool
{
    private readonly ToolType type = ToolType.Digging;
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

    public new void UseTool(Ray ray, RaycastHit hit)
    {
        base.UseTool(ray, hit);

        // todo: digs up dirt
    }

    public new void DropItem(Vector3 position, Vector3 direction)
    {
        // todo: drop tool
        base.DropItem(position, direction);
        rb.AddForce(direction * dropStrength);
        Debug.Log("Dropped digging tool.");
    }
}
