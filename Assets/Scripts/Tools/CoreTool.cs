using UnityEngine;

public class CoreTool : MonoBehaviour, ITool
{
    protected Rigidbody rb;
    protected float dropStrength = 1000f;

    protected IInteractable interactable;
    [SerializeField] protected ToolType type;

    public ToolType Type { get => type; }

    void LateUpdate()
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
        gameObject.SetActive(false);
    }

    public void UseTool(Vector3 origin, Vector3 direction, float distance)
    {
        Ray ray = new Ray(origin, direction * distance);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            interactable = hit.transform.GetComponent<IInteractable>();

            if(interactable != null)
                interactable.Interact(this);
        }
    }
}
