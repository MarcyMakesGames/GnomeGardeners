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
    #endregion

    #region Initialization
    public void InitializePlayer(PlayerConfig playerConfig)
    {
        Debug.Log("Initializing gnome.");
        this.playerConfig = playerConfig;
        //This is where we would initialize the gnome skin.
        //skin = playerConfig.skin;

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

    private void Update() => Interact();

    private void Move()
    {
        if (moveDir != Vector2.zero)
            lookDir = moveDir;
        transform.position += (Vector3)moveDir * moveSpeed * Time.deltaTime;
    }

    private void Interact()
    {
        var currentCell = GameManager.Instance.GridManager.GetClosestCell(transform.position);
        var interactionPosition = currentCell.GridPosition + lookDir * interactRange;
        var interactionCell = GameManager.Instance.GridManager.GetClosestCell(interactionPosition);

        //if (tool != null)
        //{
        //    tool.UseTool(interactionCell);
        //    return;

        //}
        //else if (tool == null)
        //{
        //    Tool interactTool = null;
        //    if (interactionCell.Occupant.GameObject.GetComponent<Tool>() != null)
        //    {
        //        interactTool = (Tool)interactionCell.Occupant;
        //    }

        //    if (interactTool != null)
        //    {
        //        DropTool();
        //        ChangeArm(interactTool);
        //    }

        //    IInteractable interactable = null;
        //    if (interactionCell.Occupant.GameObject.GetComponent<IInteractable>() != null)
        //    {
        //        interactable = (IInteractable)interactionCell.Occupant;
        //    }
        //    if (interactable != null)
        //    {
        //        interactable.Interact();
        //    }
        //}
    }

    private void ChangeArm(Tool tool)
    {
        // todo: change animation sprite
        this.tool = tool;
        var renderer = tool.GetComponent<SpriteRenderer>();
        skin.ChangeArm(renderer);
    }

    private void DropTool()
    {
        if (tool == null)
        {
            return;
        }

        var dropPosition = transform.position + (Vector3)lookDir * dropRange;
        var dropCell = GameManager.Instance.GridManager.GetClosestCell(dropPosition);
        tool.Unequip(dropCell);
        skin.ResetArm();
        tool = null;
    }

    #endregion

}
