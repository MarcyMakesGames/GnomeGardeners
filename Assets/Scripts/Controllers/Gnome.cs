using UnityEngine;
using UnityEngine.InputSystem;

public class Gnome : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float interactRange = 5f;
    public ITool tool;
    private Vector2 direction = Vector2.zero;
    private Vector3 velocity;
    private Rigidbody body;
    private bool isUsingTool = false;
    private Vector3 interactDirection = Vector3.back;

    void Start()
    {
        velocity = new Vector3(speed, 0f, speed);
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        velocity = new Vector3(direction.x * speed, 0f, direction.y * speed);
        body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        interactDirection = new Vector3(direction.x > 0f ? 1f : 0f, 0f, direction.y > 0f ? 1f : 0f);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // todo: interact with tools
        RaycastHit hit;
        if(tool != null)
        {
            isUsingTool = true;
            tool.UseTool();
        }
        else if(Physics.Raycast(transform.position, interactDirection, out hit, interactRange))
        {
            
        }
    }
}
