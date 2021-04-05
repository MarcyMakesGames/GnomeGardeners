using UnityEngine;

public class WateringTool : CoreTool, ITool
{
    [SerializeField]
    private float waterAmount;
    private SpriteRenderer spriteRenderer;

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

    public new void UseTool(Vector2 usePosition, Vector2 useDirection, float useRange)
    {
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
