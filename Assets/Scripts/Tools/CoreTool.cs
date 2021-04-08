using UnityEngine;

public class CoreTool : MonoBehaviour, ITool
{
    protected IInteractable interactable;
    protected Transform lastHitTransform;
    [SerializeField] protected ToolType type;

    protected int objectIndex;

    public ToolType Type { get => type; }

    public string Name => throw new System.NotImplementedException();

    public int ObjectIndex { get => objectIndex; set => objectIndex = value; }

    public void LateUpdate()
    {
        interactable = null;
    }

    public void DropItem(Vector2 position)
    {
        // todo: drop tool
        gameObject.SetActive(true);
        transform.position = position;
    }

    public void Interact(ITool tool = null)
    {
        // todo: gets equipped
        if(tool == null)
        {
            gameObject.SetActive(false);
            Debug.Log("Equipped Tool: " + Type.ToString());
        } 
        else
        {
            switch (tool.Type)
            {
                case ToolType.Carrying:
                    // pick up with carrying tool
                    gameObject.SetActive(false);
                    Debug.Log("Carrying tool: " + Type.ToString());
                    break;
                case ToolType.Digging:
                case ToolType.Watering:
                case ToolType.Harvesting:
                default:
                    break;
            }
        }
    }

    public void UseTool(Vector3 origin, Vector3 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Interactable"));
        Debug.Log("Raycasting in " + direction + " direction from " + transform.position + " position.");

        if (hit.collider != null)
        {
            interactable = hit.transform.GetComponent<IInteractable>();

            if(interactable != null)
                interactable.Interact(this);

            lastHitTransform = hit.transform;
        }
    }
}
