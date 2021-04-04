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
    public void Interact(ITool tool = null)
    {
        // todo: gets equipped
        Destroy(gameObject);
    }

    public void UseTool()
    {
        // todo: waters plants
    }
    public void DropItem(Vector3 position)
    {
        // todo: drop tool
        Instantiate(gameObject, position, transform.rotation);

    }
}
