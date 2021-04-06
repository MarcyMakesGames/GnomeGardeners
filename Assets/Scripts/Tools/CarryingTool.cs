using UnityEngine;

public class CarryingTool : CoreTool, ITool
{
    private GameObject heldItem = null;

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

    public new void UseTool(Ray ray, RaycastHit hit)
    {
        if (heldItem != null)
        {
            heldItem.GetComponent<IHeldItem>().DropItem(ray.origin, ray.direction);
        }

        if (hit.transform.GetComponent<IHeldItem>() != null)
        {
            heldItem = hit.transform.gameObject;
        }
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
