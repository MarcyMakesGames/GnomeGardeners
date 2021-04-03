using UnityEngine;

public class CarryingTool : MonoBehaviour, ITool
{

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        // todo: gets equipped
    }

    public void UseTool()
    {
        // todo: pick up object IHeldItem in front of gnome
    }
}
