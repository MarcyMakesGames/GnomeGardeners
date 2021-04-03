using UnityEngine;

public class WateringTool : MonoBehaviour, ITool
{
    [SerializeField]
    private float waterAmount;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        waterAmount = 25f;
            
    }

    public void Interact()
    {
        // todo: gets equipped
    }

    public void UseTool()
    {
        // todo: waters plant in front of gnome
    }

    public void DropItem()
    {
        // todo: drop item on ground
    }
}
