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
        Destroy(gameObject);
        Debug.Log("Equipped carrying tool.");
    }

    public void UseTool()
    {
        // todo: pick up object IHeldItem in front of gnome
    }
    public void DropItem(Vector3 position)
    {
        // todo: drop tool
        Instantiate(gameObject, position, transform.rotation);
        Debug.Log("Dropped carrying tool.");
    }
}
