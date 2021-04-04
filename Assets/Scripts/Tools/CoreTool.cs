using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreTool : MonoBehaviour, ITool
{
    public void DropItem(Vector3 position)
    {
        // todo: drop tool
        Instantiate(gameObject, position, transform.rotation);
    }

    public void Interact(ITool tool = null)
    {
        // todo: gets equipped
        Destroy(gameObject);
    }

    public void UseTool()
    {
        //Perhaps a change to UseTool to include the raycasting origin/direction/range?
    }

    public void UseTool(Vector2 usePosition, Vector2 useDirection, float useRange)
    {
        LayerMask interactableMask = LayerMask.GetMask("Interactable");
        RaycastHit hit;

        if (Physics.Raycast(usePosition, useDirection, out hit, useRange, interactableMask))
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
