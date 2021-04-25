using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class GnomeController : MonoBehaviour
{
    private bool debug = false;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private float dropRange = 1f;
    //Here we want a skin for the gnomes
    private GnomeSkin skin;

    private Tool tool;
    private Vector2 moveDir = Vector2.zero;
    private Vector2 lookDir;

    private PlayerConfig playerConfig;
    private GnomeInput inputs;
    private CameraFollow cameraFollow;
    private bool interacting = false;

    private GridCell interactionCell;

    public Vector2 LookDir { get => lookDir; }
    public Tool EquippedTool { get => tool; set => tool = value; }

    #region InputEvents
    public void OnInputAction(CallbackContext context)
    {
        Debug.Log("R/x input");
        if (context.action.name == inputs.Player.Movement.name)
            OnInputMove(context);
    }

    private void OnInputMove(CallbackContext context)
    {
        Debug.Log("R/x move input");
        moveDir = context.ReadValue<Vector2>();
    }

    private void OnInputEquipUnequip(CallbackContext context)
    {
        Log("Equip/Unequip action triggered.");
        if (context.started)
            EquipUnequip(interactionCell);
    }

    private void OnInputUseTool(CallbackContext context)
    {
        Log("Use tool action triggered.");
        if (context.started)
            UseTool(interactionCell);
    }

    #endregion

    #region Initialization
    public void InitializePlayer(PlayerConfig playerConfig)
    {
        Debug.Log("Initializing gnome.");
        this.playerConfig = playerConfig;
        //This is where we would initialize the gnome skin.
        //skin = playerConfig.skin;
        skin = gameObject.GetComponent<GnomeSkin>();

        playerConfig.Input.onActionTriggered += OnInputAction;
    }

    private void Awake()
    {
        inputs = new GnomeInput();
        cameraFollow = FindObjectOfType<CameraFollow>();
    }

    private void Start()
    {
        cameraFollow.target = this.transform;
    }
    #endregion

    #region Private Methods

    private void FixedUpdate() => Move();

    private void Update()
    {
        interactionCell = CalculateInteractionCell();
        GameManager.Instance.GridManager.HighlightTile(interactionCell.GridPosition);
    }


    private void Move()
    {
        if (moveDir != Vector2.zero)
            lookDir = moveDir;
        transform.position += (Vector3)moveDir * moveSpeed * Time.deltaTime;
    }

    private void UseTool(GridCell cell)
    {
        Log("Interacting.");
        var occupant = cell.Occupant;
        GameManager.Instance.GridManager.FlashHighlightTile(cell.GridPosition);

        if (tool != null) // note: tool equipped and interacting on cell
        {
            tool.UseTool(cell);
        }
        else if (tool == null && occupant != null) // note: no Tool equipped and interacting on occupant
        {
            IInteractable interactableOnGround = null;
            if (occupant.AssociatedObject.GetComponent<IInteractable>() != null)
            {
                interactableOnGround = (IInteractable)occupant;
            }
            if (interactableOnGround != null)
            {
                interactableOnGround.Interact();
            }
        }
    }

    private GridCell CalculateInteractionCell()
    {
        var currentCell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
        var interactionPosition = currentCell.GridPosition + lookDir * interactRange;
        var interactionCell = GameManager.Instance.GridManager.GetClosestCell(interactionPosition);
        return interactionCell;
    }

    private void EquipUnequip(GridCell cell)
    {
        var occupant = cell.Occupant;
        GameManager.Instance.GridManager.FlashHighlightTile(cell.GridPosition);

        if(tool != null && occupant != null)
        {
            Log("Cannot drop tool on occupied tile.");
        }
        else if (tool != null && occupant == null)
        {
            UnequipTool();
        }
        else if (tool == null && occupant != null) // note: no Tool equipped and interacting on occupant
        {
            Tool toolOnGround = null;
            if (occupant.AssociatedObject.GetComponent<Tool>() != null)
            {
                toolOnGround = (Tool)occupant;
            }

            if (toolOnGround != null)
            {
                EquipTool(toolOnGround, cell);
            }
        }
    }

    private void EquipTool(Tool tool, GridCell cell)
    {
        // todo: change animation sprite
        this.tool = tool;
        var renderer = tool.GetComponent<SpriteRenderer>();
        skin.ChangeArm(renderer);
        tool.Equip(cell);
    }

    private void UnequipTool()
    {
        if (tool == null)
            return;

        var dropPosition = transform.position + (Vector3)lookDir * dropRange;
        var dropCell = GameManager.Instance.GridManager.GetClosestCell(dropPosition);
        tool.Unequip(dropCell);
        skin.ResetArm();
        tool = null;
    }

    private void Log(string msg)
    {
        if (debug)
            Debug.Log("[GnomeController]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (debug)
            Debug.LogWarning("[GnomeController]: " + msg);
    }

    #endregion

}
