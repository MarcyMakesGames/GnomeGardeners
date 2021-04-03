using UnityEngine;
using UnityEngine.InputSystem;

public class Gnome : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Vector2 direction;
    private Vector3 velocity;
    private Rigidbody body;
    public ITool tool; 

    void Start()
    {
        direction = Vector2.zero;
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
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        // todo: interact with tools
    }
}
