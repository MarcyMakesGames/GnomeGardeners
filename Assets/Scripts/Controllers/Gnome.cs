using UnityEngine;
using UnityEngine.InputSystem;

public class Gnome : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float interactRange = 5f;
    public ITool activeTool;

    private Vector2 direction = Vector2.zero;
    private Vector3 velocity;
    private Rigidbody body;
    private bool canMove = true;
    private Vector3 interactDirection = Vector3.back;
    private GnomeSkin skin;
    private CoreTool toolObject;

    void Awake()
    {
        velocity = new Vector3(speed, 0f, speed);
        body = GetComponent<Rigidbody>();
        skin = GetComponent<GnomeSkin>();
    }

    void FixedUpdate()
    {
        velocity = new Vector3(direction.x * speed, 0f, direction.y * speed);
        body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
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
            interactDirection.z = direction.y > 0f ? 1f : -1f;
        }
        else if (direction.x != 0 && direction.y == 0)
        {
            interactDirection.x = direction.x > 0f ? 1f : -1f;
            interactDirection.z = direction.y;
        }
        else if (direction.x == 0 && direction.y != 0)
        {
            interactDirection.x = direction.x;
            interactDirection.z = direction.y > 0f ? 1f : -1f;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // todo: interact with tools
        if (context.performed != true)
            return;


        if (activeTool != null)
        {
            activeTool.UseTool(transform.position, direction, interactRange);
            return;
        }

        else if(activeTool == null)
        {
            Ray ray = new Ray(transform.position, direction * interactRange);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
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

        activeTool.DropItem(transform.position + interactDirection, interactDirection);
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
