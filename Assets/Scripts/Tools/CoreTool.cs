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

    public void UseTool(Ray ray, RaycastHit hit)
    {
        if (hit.transform.GetComponent<IInteractable>() != null)
        {
            interactable = hit.transform.GetComponent<IInteractable>();
            interactable.Interact(this);
        }

        // todo: animation work, sfx, etc.
    }
}
