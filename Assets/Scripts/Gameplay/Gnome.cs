using UnityEngine;
using UnityEngine.InputSystem;

public class Gnome : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private float dropRange = 1f;
    public ITool activeTool;

    private Vector2 direction = Vector2.zero;
    private Vector2 interactDirection = Vector2.down;
    private Vector3 velocity;
    private bool canMove = true;
    private GnomeSkin skin;
    private CoreTool toolObject;

    void Awake()
    {
        velocity = new Vector3(0f, 0f, 0f);
        skin = GetComponent<GnomeSkin>();

    }

    void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y = direction.y * speed;
        transform.Translate(velocity, Space.World);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            return;
        }
        direction = context.ReadValue<Vector2>();

        CalculateInteractDirection();
    }

    private void CalculateInteractDirection()
    {
        if (direction.x != 0 && direction.y != 0)
        {
            interactDirection.x = direction.x > 0f ? 1f : -1f;
            interactDirection.y = direction.y > 0f ? 1f : -1f;
        }
        else if (direction.x != 0 && direction.y == 0)
        {
            interactDirection.x = direction.x > 0f ? 1f : -1f;
            interactDirection.y = direction.y;
        }
        else if (direction.x == 0 && direction.y != 0)
        {
            interactDirection.x = direction.x;
            interactDirection.y = direction.y > 0f ? 1f : -1f;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // todo: interact with tools
        if (context.performed != true)
            return;

        if (activeTool != null)
        {
            activeTool.UseTool(transform.position, interactDirection, interactRange);
            return;

        }
        else if(activeTool == null)
        {
            Ray2D ray = new Ray2D(transform.position, interactDirection);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, interactRange, LayerMask.GetMask("Interactable"));
            Debug.Log("Raycasting in " + interactDirection + " direction from " + transform.position + " position.");

            if (hit.collider != null)
            {
                Debug.DrawLine(ray.origin, ray.direction);
                Debug.Log("Found " + hit.transform.gameObject.name);
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                ITool tool = hit.transform.GetComponent<ITool>();

                if (tool != null)
                {
                    DropTool();
                    ChangeArm(tool);
                    tool.Interact();
                }

                if(interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    public void OnDropItem(InputAction.CallbackContext context)
    {
        if (context.performed != true)
            return;
        DropTool();
    }

    private void DropTool()
    {
        if (activeTool == null)
        {
            return;
        }
        
        var dropPosition = (Vector2)transform.position + interactDirection * dropRange;
        activeTool.DropItem(dropPosition);
        skin.ResetArm();
        activeTool = null;
    }

    public void ChangeArm(ITool tool)
    {
        // todo: change animation sprite
        activeTool = tool;
        toolObject = (CoreTool)tool;
        SpriteRenderer renderer = toolObject.GetComponent<SpriteRenderer>();
        skin.ChangeArm(renderer);
    }

}
