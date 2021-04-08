using UnityEngine;

public class WateringTool : CoreTool, ITool
{
    [SerializeField] private float waterAmount;
    [SerializeField] protected bool is2D;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        waterAmount = 25f;
    }

    public new void Interact(ITool tool = null)
    {
        // todo: gets equipped
        base.Interact(tool);
    }

    public new void UseTool(Vector3 origin, Vector3 direction, float distance)
    {
        if(is2D)
        {
            base.UseTool(origin, direction, distance);
        }

        else
        {
            base.UseTool(origin, direction, distance);
        }
    }

    public new void DropItem(Vector2 position)
    {
        // todo: drop tool
        base.DropItem(position);
        Debug.Log("Dropped watering tool.");
    }
}
