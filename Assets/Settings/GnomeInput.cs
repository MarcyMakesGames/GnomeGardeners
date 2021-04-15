// GENERATED AUTOMATICALLY FROM 'Assets/Settings/GnomeInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GnomeInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GnomeInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GnomeInput"",
    ""maps"": [
        {
            ""name"": ""GnomeKeyboardLeft"",
            ""id"": ""e3901da0-4b07-4296-a467-8c35d8446e09"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""5e348309-67ea-41a3-a806-d26b387a1376"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""3613c754-fa9e-4f1f-bbf9-e613e2fd829d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tool"",
                    ""type"": ""Button"",
                    ""id"": ""9368d4e2-9e54-4c1d-b236-088989f542b3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard WASD Move"",
                    ""id"": ""c59c38ef-11a7-45f3-8b8a-96eef43f0595"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c27b837e-7379-4543-89d3-fac1ad74dbd4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e86ab747-7119-448d-8b47-4bf78489f950"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""29859396-9ffe-4ed7-b261-cf352c5f3240"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e2cac156-783e-4e96-8668-edd2d3957042"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d34311f1-51a8-46c3-92fc-f0273de8a841"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""46a865a3-675b-4c1f-9119-a7dc3d197a37"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d599112-b6b5-4c70-a23f-8a9ff5d2652d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4acb12b-3b0b-467f-920c-0f2c80e2d96b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Left"",
                    ""action"": ""Tool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62755b06-0c5d-48fc-ab2a-e84cf6e7bb5e"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Tool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GnomeKeyboardRight"",
            ""id"": ""c1e7489d-8010-453d-8b5c-9753f89eeb77"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""c7574f42-c2ea-4252-8cb2-36ec3c4e0c2c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""3da5650b-2a63-4124-a4fe-7457a48da6f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tool"",
                    ""type"": ""Button"",
                    ""id"": ""73791a39-d939-40ea-a8d6-c19ff6e25eae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard IJKL Move"",
                    ""id"": ""e116d364-d24f-49d4-9c3a-8f7db758e76e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""384e9aa5-8324-40b2-856a-1e39254e4e62"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Right"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f2d97ca1-e84d-456d-83a2-a6283b288d24"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Right"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6dc2fd61-9fe3-4c11-8f2d-a6f4270bc42c"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Right"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""79eafaff-6341-4dd6-a573-26dfe85fad84"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Right"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""73591efc-d60f-453a-9399-a451e1a45a7c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13ea1eb1-0334-4401-94ed-4a96e9cf6f28"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Right"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2fd87c7d-02f1-49cc-8942-5c0d1aeb0ffb"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd640144-0250-40f7-bdef-02c4395995a6"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Right"",
                    ""action"": ""Tool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25fada45-1402-491c-9291-137deaa9268b"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Tool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GnomeGamepad"",
            ""id"": ""43c7a681-524f-4605-81b1-a1d42da7ff25"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""2efed14c-a295-4344-b162-43b614e71bcd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""70288fbd-33f7-4c24-8f8c-707f1ef7e7c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tool"",
                    ""type"": ""Button"",
                    ""id"": ""741963c3-d578-40f2-b0fb-8ef8bb8c8d07"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6cd165ac-738f-405c-9c74-1da0ddc4503f"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0bced40-abdc-4199-af07-30e44ff1bbc5"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""197cc36a-c40b-42df-be49-1fa43f907a10"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Tool"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard Left"",
            ""bindingGroup"": ""Keyboard Left"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard Right"",
            ""bindingGroup"": ""Keyboard Right"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // GnomeKeyboardLeft
        m_GnomeKeyboardLeft = asset.FindActionMap("GnomeKeyboardLeft", throwIfNotFound: true);
        m_GnomeKeyboardLeft_Movement = m_GnomeKeyboardLeft.FindAction("Movement", throwIfNotFound: true);
        m_GnomeKeyboardLeft_Interaction = m_GnomeKeyboardLeft.FindAction("Interaction", throwIfNotFound: true);
        m_GnomeKeyboardLeft_Tool = m_GnomeKeyboardLeft.FindAction("Tool", throwIfNotFound: true);
        // GnomeKeyboardRight
        m_GnomeKeyboardRight = asset.FindActionMap("GnomeKeyboardRight", throwIfNotFound: true);
        m_GnomeKeyboardRight_Movement = m_GnomeKeyboardRight.FindAction("Movement", throwIfNotFound: true);
        m_GnomeKeyboardRight_Interaction = m_GnomeKeyboardRight.FindAction("Interaction", throwIfNotFound: true);
        m_GnomeKeyboardRight_Tool = m_GnomeKeyboardRight.FindAction("Tool", throwIfNotFound: true);
        // GnomeGamepad
        m_GnomeGamepad = asset.FindActionMap("GnomeGamepad", throwIfNotFound: true);
        m_GnomeGamepad_Movement = m_GnomeGamepad.FindAction("Movement", throwIfNotFound: true);
        m_GnomeGamepad_Interaction = m_GnomeGamepad.FindAction("Interaction", throwIfNotFound: true);
        m_GnomeGamepad_Tool = m_GnomeGamepad.FindAction("Tool", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // GnomeKeyboardLeft
    private readonly InputActionMap m_GnomeKeyboardLeft;
    private IGnomeKeyboardLeftActions m_GnomeKeyboardLeftActionsCallbackInterface;
    private readonly InputAction m_GnomeKeyboardLeft_Movement;
    private readonly InputAction m_GnomeKeyboardLeft_Interaction;
    private readonly InputAction m_GnomeKeyboardLeft_Tool;
    public struct GnomeKeyboardLeftActions
    {
        private @GnomeInput m_Wrapper;
        public GnomeKeyboardLeftActions(@GnomeInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_GnomeKeyboardLeft_Movement;
        public InputAction @Interaction => m_Wrapper.m_GnomeKeyboardLeft_Interaction;
        public InputAction @Tool => m_Wrapper.m_GnomeKeyboardLeft_Tool;
        public InputActionMap Get() { return m_Wrapper.m_GnomeKeyboardLeft; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GnomeKeyboardLeftActions set) { return set.Get(); }
        public void SetCallbacks(IGnomeKeyboardLeftActions instance)
        {
            if (m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface.OnMovement;
                @Interaction.started -= m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface.OnInteraction;
                @Tool.started -= m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface.OnTool;
                @Tool.performed -= m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface.OnTool;
                @Tool.canceled -= m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface.OnTool;
            }
            m_Wrapper.m_GnomeKeyboardLeftActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @Tool.started += instance.OnTool;
                @Tool.performed += instance.OnTool;
                @Tool.canceled += instance.OnTool;
            }
        }
    }
    public GnomeKeyboardLeftActions @GnomeKeyboardLeft => new GnomeKeyboardLeftActions(this);

    // GnomeKeyboardRight
    private readonly InputActionMap m_GnomeKeyboardRight;
    private IGnomeKeyboardRightActions m_GnomeKeyboardRightActionsCallbackInterface;
    private readonly InputAction m_GnomeKeyboardRight_Movement;
    private readonly InputAction m_GnomeKeyboardRight_Interaction;
    private readonly InputAction m_GnomeKeyboardRight_Tool;
    public struct GnomeKeyboardRightActions
    {
        private @GnomeInput m_Wrapper;
        public GnomeKeyboardRightActions(@GnomeInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_GnomeKeyboardRight_Movement;
        public InputAction @Interaction => m_Wrapper.m_GnomeKeyboardRight_Interaction;
        public InputAction @Tool => m_Wrapper.m_GnomeKeyboardRight_Tool;
        public InputActionMap Get() { return m_Wrapper.m_GnomeKeyboardRight; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GnomeKeyboardRightActions set) { return set.Get(); }
        public void SetCallbacks(IGnomeKeyboardRightActions instance)
        {
            if (m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface.OnMovement;
                @Interaction.started -= m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface.OnInteraction;
                @Tool.started -= m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface.OnTool;
                @Tool.performed -= m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface.OnTool;
                @Tool.canceled -= m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface.OnTool;
            }
            m_Wrapper.m_GnomeKeyboardRightActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @Tool.started += instance.OnTool;
                @Tool.performed += instance.OnTool;
                @Tool.canceled += instance.OnTool;
            }
        }
    }
    public GnomeKeyboardRightActions @GnomeKeyboardRight => new GnomeKeyboardRightActions(this);

    // GnomeGamepad
    private readonly InputActionMap m_GnomeGamepad;
    private IGnomeGamepadActions m_GnomeGamepadActionsCallbackInterface;
    private readonly InputAction m_GnomeGamepad_Movement;
    private readonly InputAction m_GnomeGamepad_Interaction;
    private readonly InputAction m_GnomeGamepad_Tool;
    public struct GnomeGamepadActions
    {
        private @GnomeInput m_Wrapper;
        public GnomeGamepadActions(@GnomeInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_GnomeGamepad_Movement;
        public InputAction @Interaction => m_Wrapper.m_GnomeGamepad_Interaction;
        public InputAction @Tool => m_Wrapper.m_GnomeGamepad_Tool;
        public InputActionMap Get() { return m_Wrapper.m_GnomeGamepad; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GnomeGamepadActions set) { return set.Get(); }
        public void SetCallbacks(IGnomeGamepadActions instance)
        {
            if (m_Wrapper.m_GnomeGamepadActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GnomeGamepadActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GnomeGamepadActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GnomeGamepadActionsCallbackInterface.OnMovement;
                @Interaction.started -= m_Wrapper.m_GnomeGamepadActionsCallbackInterface.OnInteraction;
                @Interaction.performed -= m_Wrapper.m_GnomeGamepadActionsCallbackInterface.OnInteraction;
                @Interaction.canceled -= m_Wrapper.m_GnomeGamepadActionsCallbackInterface.OnInteraction;
                @Tool.started -= m_Wrapper.m_GnomeGamepadActionsCallbackInterface.OnTool;
                @Tool.performed -= m_Wrapper.m_GnomeGamepadActionsCallbackInterface.OnTool;
                @Tool.canceled -= m_Wrapper.m_GnomeGamepadActionsCallbackInterface.OnTool;
            }
            m_Wrapper.m_GnomeGamepadActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @Tool.started += instance.OnTool;
                @Tool.performed += instance.OnTool;
                @Tool.canceled += instance.OnTool;
            }
        }
    }
    public GnomeGamepadActions @GnomeGamepad => new GnomeGamepadActions(this);
    private int m_KeyboardLeftSchemeIndex = -1;
    public InputControlScheme KeyboardLeftScheme
    {
        get
        {
            if (m_KeyboardLeftSchemeIndex == -1) m_KeyboardLeftSchemeIndex = asset.FindControlSchemeIndex("Keyboard Left");
            return asset.controlSchemes[m_KeyboardLeftSchemeIndex];
        }
    }
    private int m_KeyboardRightSchemeIndex = -1;
    public InputControlScheme KeyboardRightScheme
    {
        get
        {
            if (m_KeyboardRightSchemeIndex == -1) m_KeyboardRightSchemeIndex = asset.FindControlSchemeIndex("Keyboard Right");
            return asset.controlSchemes[m_KeyboardRightSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IGnomeKeyboardLeftActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnTool(InputAction.CallbackContext context);
    }
    public interface IGnomeKeyboardRightActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnTool(InputAction.CallbackContext context);
    }
    public interface IGnomeGamepadActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnTool(InputAction.CallbackContext context);
    }
}
