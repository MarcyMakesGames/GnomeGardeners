using UnityEngine;
using UnityEngine.InputSystem;

public class Gnome : MonoBehaviour
{
    [SerializeField]
    private float gnomeSpeed = 2f;

    private Vector2 gnomeVelocity = new Vector2(0f, 0f);

    private Vector2 movementInput = Vector2.zero;

    void Update()
    {
        Vector2 move = new Vector2(movementInput.x, movementInput.y);
        transform.position += new Vector3(move.x * Time.deltaTime * gnomeSpeed, move.y * Time.deltaTime * gnomeSpeed, 0f);
        transform.position += new Vector3(gnomeVelocity.x * Time.deltaTime, gnomeVelocity.y * Time.deltaTime, 0f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
}
