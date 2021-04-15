using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GnomeController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GnomeInput gnomeInput;
    [SerializeField] private ControlScheme gnomeControlScheme;

    private Vector2 lookDir;
    private Vector2 moveDir = Vector2.zero;
    private bool interacting = false;
    private ITool tool;

    public GnomeInput GnomeInput { get => gnomeInput; set => gnomeInput = value; }
    public ControlScheme GnomeControlScheme { get => gnomeControlScheme; set => gnomeControlScheme = value; }
    public Vector2 LookDir { get => lookDir; }
    public ITool EquippedTool { get => tool; set => tool = value; }

    public GnomeController(GnomeInput gnomeInput, ControlScheme controlScheme)
    {
        this.gnomeInput = gnomeInput;
        gnomeControlScheme = controlScheme;
    }

    public void OnMoveInput(InputAction.CallbackContext moveContext) => moveDir = moveContext.ReadValue<Vector2>();
    public void OnInteractInput(InputAction.CallbackContext interactContext) => interacting = interactContext.action.triggered;


    private void OnEnable()
    {
        if (gnomeInput == null)
            gnomeInput = new GnomeInput();

        switch(gnomeControlScheme)
        {
            case ControlScheme.KeyboardLeft:
                gnomeInput.GnomeKeyboardLeft.Enable();
                gnomeInput.GnomeKeyboardLeft.Movement.performed += OnMoveInput;
                gnomeInput.GnomeKeyboardLeft.Movement.canceled += OnMoveInput;
                gnomeInput.GnomeKeyboardLeft.Interaction.performed += OnInteractInput;
                break;
            case ControlScheme.KeyboardRight:
                gnomeInput.GnomeKeyboardRight.Enable();
                gnomeInput.GnomeKeyboardRight.Movement.performed += OnMoveInput;
                gnomeInput.GnomeKeyboardRight.Movement.canceled += OnMoveInput;
                gnomeInput.GnomeKeyboardRight.Interaction.performed += OnInteractInput;
                break;
            case ControlScheme.GamepadOnly:
                gnomeInput.GnomeGamepad.Enable();
                gnomeInput.GnomeGamepad.Movement.performed += OnMoveInput;
                gnomeInput.GnomeGamepad.Movement.canceled += OnMoveInput;
                gnomeInput.GnomeGamepad.Interaction.performed += OnInteractInput;
                break;
        }
    }

    private void OnDisable()
    {
        switch (gnomeControlScheme)
        {
            case ControlScheme.KeyboardLeft:
                gnomeInput.GnomeKeyboardLeft.Enable();
                gnomeInput.GnomeKeyboardLeft.Movement.performed -= OnMoveInput;
                gnomeInput.GnomeKeyboardLeft.Movement.canceled -= OnMoveInput;
                gnomeInput.GnomeKeyboardLeft.Interaction.performed -= OnInteractInput;
                break;
            case ControlScheme.KeyboardRight:
                gnomeInput.GnomeKeyboardRight.Enable();
                gnomeInput.GnomeKeyboardRight.Movement.performed -= OnMoveInput;
                gnomeInput.GnomeKeyboardRight.Movement.canceled -= OnMoveInput;
                gnomeInput.GnomeKeyboardRight.Interaction.performed -= OnInteractInput;
                break;
            case ControlScheme.GamepadOnly:
                gnomeInput.GnomeGamepad.Enable();
                gnomeInput.GnomeGamepad.Movement.performed -= OnMoveInput;
                gnomeInput.GnomeGamepad.Movement.canceled -= OnMoveInput;
                gnomeInput.GnomeGamepad.Interaction.performed -= OnInteractInput;
                break;
        }
    }

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

public enum ControlScheme
{
    KeyboardLeft,
    KeyboardRight,
    GamepadOnly
}
