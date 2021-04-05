using UnityEngine;

public class CoreTool : MonoBehaviour, ITool
{
    protected Rigidbody rb;
    protected float dropStrength = 1000f;

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

    public void UseTool(Vector2 usePosition, Vector2 useDirection, float useRange)
    {
        LayerMask interactableMask = LayerMask.GetMask("Interactable");

        if (Physics.Raycast(usePosition, useDirection, out RaycastHit hit, useRange, interactableMask))
        {
            if (hit.transform.GetComponent<IInteractable>() != null)
            {
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                interactable.Interact(this);
            }

            // todo: animation work, sfx, etc.
        }
    }
}
