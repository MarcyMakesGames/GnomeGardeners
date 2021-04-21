using UnityEngine;
using UnityEngine.InputSystem;

public class Gnome : MonoBehaviour
{
    private bool debug = false;

    [SerializeField] private float speed;
    [SerializeField] private int interactRange = 1;
    [SerializeField] private int dropRange = 1;

    public Tool activeTool;
    private Vector2 direction = Vector2.zero;
    private Vector3Int interactDirection = new Vector3Int(1, 0, 0);
    private Vector3 velocity;


    private bool canMove = true;
    private GnomeSkin skin;

    #region Unity Methods
    void Awake()
    {
        velocity = new Vector3(0f, 0f, 0f);
        skin = GetComponent<GnomeSkin>();

    }

    void Start()
    {
        if (debug)
        {
            Debug.Log("Gnome: Debug activated");
        }
    }

    void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y = direction.y * speed;
        transform.Translate(velocity, Space.World);
    }
    #endregion

    #region Public Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            return;
        }
        direction = context.ReadValue<Vector2>();

        CalculateInteractDirection();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed != true) // note: necessary for calling method once per key press
            return;

        if (debug)
        {
            var toolObject = GameObject.FindWithTag("DEBUG");
            var tool = toolObject.GetComponent<Tool>();
            ChangeArm(tool);
        }

        var currentCell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
        var interactionPosition = currentCell.GridPosition + interactDirection * interactRange;
        var interactionCell = GameManager.Instance.GridManager.GetGridCell(interactionPosition);

        if (activeTool != null)
        {
            activeTool.UseTool(interactionCell);
            return;

        }
        else if (activeTool == null)
        {
            Tool interactTool = null;
            if (interactionCell.Occupant.GameObject.GetComponent<Tool>() != null)
            {
                interactTool = (Tool)interactionCell.Occupant;
            }

            if (interactTool != null)
            {
                DropTool();
                ChangeArm(interactTool);
            }

            IInteractable interactable = null;
            if (interactionCell.Occupant.GameObject.GetComponent<IInteractable>() != null)
            {
                interactable = (IInteractable)interactionCell.Occupant;
            }
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    public void OnDropItem(InputAction.CallbackContext context)
    {
        if (context.performed != true)
            return;
        DropTool();
    }

    public void ChangeArm(Tool tool)
    {
        // todo: change animation sprite
        activeTool = tool;
        SpriteRenderer renderer = activeTool.GetComponent<SpriteRenderer>();
        skin.ChangeArm(renderer);
    }
    #endregion

    #region Private Methods
    private void CalculateInteractDirection()
    {
        if (direction.x != 0 && direction.y != 0)
        {
            interactDirection.x = direction.x > 0 ? 1 : -1;
            interactDirection.y = direction.y > 0 ? 1 : -1;
        }
        else if (direction.x != 0 && direction.y == 0)
        {
            interactDirection.x = direction.x > 0 ? 1 : -1;
            interactDirection.y = (int)direction.y;
        }
        else if (direction.x == 0 && direction.y != 0)
        {
            interactDirection.x = (int)direction.x;
            interactDirection.y = direction.y > 0 ? 1 : -1;
        }
    }

    private void DropTool()
    {
        if (activeTool == null)
        {
            return;
        }
        
        var dropPosition = Vector3Int.FloorToInt(transform.position) + interactDirection * dropRange;
        var dropCell = GameManager.Instance.GridManager.GetGridCell(dropPosition);
        activeTool.Unequip(dropCell);
        skin.ResetArm();
        activeTool = null;
    }
    #endregion

}
