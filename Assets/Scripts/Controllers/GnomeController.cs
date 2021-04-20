using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class GnomeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    //Here we want a skin for the gnomes

    private PlayerConfig playerConfig;
    private GnomeInput inputs;
    private CameraFollow cameraFollow;
    private Vector2 lookDir;
    private Vector2 moveDir = Vector2.zero;
    private bool interacting = false;
    private ITool tool;

    public Vector2 LookDir { get => lookDir; }
    public ITool EquippedTool { get => tool; set => tool = value; }

    #region InputEvents
    public void OnInputAction(CallbackContext context)
    {
        if (context.action.name == inputs.PlayerMovement.Movement.name)
            OnInputMove(context);
    }

    private void OnInputMove(CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }
    #endregion

    #region Initialization
    public void InitializePlayer(PlayerConfig playerConfig)
    {
        this.playerConfig = playerConfig;
        //This is where we would initialize the gnome skin.

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

    private void FixedUpdate() => Move();

    private void Update() => Interact();

    private void Move()
    {
        lookDir = moveDir;
        transform.position += (Vector3)moveDir * moveSpeed * Time.deltaTime;
    }

    private void Interact()
    {
        if(interacting)
        {
            Vector3 interactDestination = transform.position + (Vector3)lookDir;
            GameObject interactable = GameManager.Instance.InteractionController.Interact(transform.position, interactDestination, tool);

            if (interactable != null)
                interactable.GetComponent<IInteractable>().Interact(tool);

            interacting = false;
        }
    }
}
