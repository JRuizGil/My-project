using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

[System.Serializable] public class ButtonEvent : UnityEvent<bool> { }
[System.Serializable] public class AxisEvent : UnityEvent<Vector2> { }

public class Quest2InputWatcher : MonoBehaviour
{
    // Eventos para cada botón
    public ButtonEvent primaryButtonPress;
    public ButtonEvent secondaryButtonPress;
    public ButtonEvent gripPress;
    public ButtonEvent triggerPress;
    public ButtonEvent joystickClick;

    // Eventos para ejes
    public AxisEvent joystickMove;

    private List<InputDevice> devices = new List<InputDevice>();

    private bool lastPrimary, lastSecondary, lastGrip, lastTrigger, lastJoyClick;

    private void Awake()
    {
        primaryButtonPress ??= new ButtonEvent();
        secondaryButtonPress ??= new ButtonEvent();
        gripPress ??= new ButtonEvent();
        triggerPress ??= new ButtonEvent();
        joystickClick ??= new ButtonEvent();
        joystickMove ??= new AxisEvent();
    }

    private void OnEnable()
    {
        InputDevices.GetDevices(devices);
        InputDevices.deviceConnected += DeviceConnected;
        InputDevices.deviceDisconnected += DeviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= DeviceConnected;
        InputDevices.deviceDisconnected -= DeviceDisconnected;
        devices.Clear();
    }

    private void DeviceConnected(InputDevice device)
    {
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
            devices.Add(device);
    }

    private void DeviceDisconnected(InputDevice device)
    {
        if (devices.Contains(device))
            devices.Remove(device);
    }

    private void Update()
    {
        foreach (var device in devices)
        {
            // Botones
            CheckButton(device, CommonUsages.primaryButton, ref lastPrimary, primaryButtonPress);
            CheckButton(device, CommonUsages.secondaryButton, ref lastSecondary, secondaryButtonPress);
            CheckButton(device, CommonUsages.gripButton, ref lastGrip, gripPress);
            CheckButton(device, CommonUsages.triggerButton, ref lastTrigger, triggerPress);
            CheckButton(device, CommonUsages.primary2DAxisClick, ref lastJoyClick, joystickClick);

            // Joystick
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joy))
                joystickMove.Invoke(joy);
        }
    }

    private void CheckButton(InputDevice device, InputFeatureUsage<bool> feature, ref bool lastState, ButtonEvent ev)
    {
        if (device.TryGetFeatureValue(feature, out bool value) && value != lastState)
        {
            ev.Invoke(value);
            lastState = value;
        }
    }
}
