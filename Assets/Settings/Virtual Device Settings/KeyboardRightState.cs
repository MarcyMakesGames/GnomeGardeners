using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// InputControlLayoutAttribute attribute is only necessary if you want
// to override default behavior that occurs when registering your device
// as a layout.
// The most common use of InputControlLayoutAttribute is to direct the system
// to a custom "state struct" through the `stateType` property. See below for details.
[InputControlLayout(displayName = "My Device", stateType = typeof(KeyboardRightState))]
public class KeyboardRight : InputDevice
{
    public ButtonControl button { get; private set; }
    public AxisControl axis { get; private set; }

    // Register the device.
    static KeyboardRight()
    {
        // In case you want instance of your device to automatically be created
        // when specific hardware is detected by the Unity runtime, you have to
        // add one or more "device matchers" (InputDeviceMatcher) for the layout.
        // These matchers are compared to an InputDeviceDescription received from
        // the Unity runtime when a device is connected. You can add them either
        // using InputSystem.RegisterLayoutMatcher() or by directly specifying a
        // matcher when registering the layout.
        InputSystem.RegisterLayout<KeyboardRight>(
            // For the sake of demonstration, let's assume your device is a HID
            // and you want to match by PID and VID.
            matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("PID", 1234)
                .WithCapability("VID", 5678));
    }

    // This is only to trigger the static class constructor to automatically run
    // in the player.
    [RuntimeInitializeOnLoadMethod]
    private static void InitializeInPlayer() { }

    protected override void FinishSetup()
    {
        base.FinishSetup();
        button = GetChildControl<ButtonControl>("button");
        axis = GetChildControl<AxisControl>("axis");
    }
}

// A "state struct" describes the memory format used by a device. Each device can
// receive and store memory in its custom format. InputControls are then connected
// the individual pieces of memory and read out values from them.
[StructLayout(LayoutKind.Explicit, Size = 32)]
public struct KeyboardRightState : IInputStateTypeInfo
{
    // In the case of a HID (which we assume for the sake of this demonstration),
    // the format will be "HID". In practice, the format will depend on how your
    // particular device is connected and fed into the input system.
    // The format is a simple FourCC code that "tags" state memory blocks for the
    // device to give a base level of safety checks on memory operations.
    public FourCC format => new FourCC('H', 'I', 'D');

    // InputControlAttributes on fields tell the input system to create controls
    // for the public fields found in the struct.

    // Assume a 16bit field of buttons. Create one button that is tied to
    // bit #3 (zero-based). Note that buttons do not need to be stored as bits.
    // They can also be stored as floats or shorts, for example.
    [InputControl(name = "button", layout = "Button", bit = 3)]
    //public ushort buttons;

    // Create a floating-point axis. The name, if not supplied, is taken from
    // the field.
    [InputControl(layout = "Axis")]
    //public short axis;
}