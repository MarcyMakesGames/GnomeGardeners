using UnityEngine;

public class CoreTool : MonoBehaviour, ITool
{
    protected Rigidbody rb;
    protected float dropStrength = 1000f;

    protected IInteractable interactable;
    protected Transform lastHitTransform;
    [SerializeField] protected ToolType type;

    public ToolType Type { get => type; }

    public string Name => throw new System.NotImplementedException();

    public void LateUpdate()
    {
        interactable = null;
    }

    public void DropItem(Vector3 position, Vector3 direction)
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
            Debug.Log("Equipped Tool");
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
        Ray ray = new Ray(origin, direction * distance);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            interactable = hit.transform.GetComponent<IInteractable>();

            if (interactable != null)
                interactable.Interact(this);

            lastHitTransform = hit.transform;

            Debug.Log("Last hit transform: " + hit.transform.gameObject.name);
        }
    }
}
