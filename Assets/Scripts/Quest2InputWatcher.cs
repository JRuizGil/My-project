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
    private float triggerThreshold = 0.6f;
    private float gripThreshold = 0.6f;    // Eventos para ejes
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
            // Botones "reales"
            CheckButton(device, CommonUsages.primaryButton, ref lastPrimary, primaryButtonPress);
            CheckButton(device, CommonUsages.secondaryButton, ref lastSecondary, secondaryButtonPress);
            CheckButton(device, CommonUsages.primary2DAxisClick, ref lastJoyClick, joystickClick);

            // --- TRIGGER ANALÓGICO ---
            if (device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                bool pressed = triggerValue > triggerThreshold;

                if (pressed != lastTrigger)
                {
                    triggerPress.Invoke(pressed);
                    lastTrigger = pressed;
                }
            }

            // --- GRIP ANALÓGICO ---
            if (device.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                bool pressed = gripValue > gripThreshold;

                if (pressed != lastGrip)
                {
                    gripPress.Invoke(pressed);
                    lastGrip = pressed;
                }
            }
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
