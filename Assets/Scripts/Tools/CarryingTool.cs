using UnityEngine;

public class CarryingTool : MonoBehaviour, ITool
{

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact(ITool tool = null)
    {
        // todo: gets equipped
        Debug.Log("Equipped carrying tool.");
        gameObject.SetActive(false);
    }

    public void UseTool()
    {
        // todo: pick up object IHeldItem in front of gnome
    }
    public void DropItem(Vector3 position)
    {
        // todo: drop tool
        gameObject.SetActive(true);
        transform.position = position;
    }
}
