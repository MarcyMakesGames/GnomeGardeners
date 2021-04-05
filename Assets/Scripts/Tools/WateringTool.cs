using UnityEngine;

public class WateringTool : CoreTool, ITool
{
    [SerializeField]
    private float waterAmount;
    private SpriteRenderer spriteRenderer;

    private readonly ToolType type = ToolType.Watering;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        waterAmount = 25f;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public new void Interact(ITool tool = null)
    {
        // todo: gets equipped
        base.Interact();
        Debug.Log("Equipped watering tool.");
    }

    public new void UseTool(Ray ray, RaycastHit hit)
    {
        base.UseTool(ray, hit);

        // todo: waters plants
    }

    public new void DropItem(Vector3 position, Vector3 direction)
    {
        // todo: drop tool
        base.DropItem(position, direction);
        rb.AddForce(direction * dropStrength);
        Debug.Log("Dropped watering tool.");
    }
}
