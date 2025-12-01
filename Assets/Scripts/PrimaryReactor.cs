using UnityEngine;
using UnityEngine.Events;

public class ButtonReactor : MonoBehaviour
{
    public Quest2InputWatcher watcher;

    [Header("Selecciona qué botón escuchar")]
    public bool listenPrimary;
    public bool listenSecondary;
    public bool listenGrip;
    public bool listenTrigger;
    public bool listenJoystickClick;

    [Header("Evento externo (ej: activar boost)")]
    public UnityEvent<bool> OnValueChanged;

    public bool isPressed;

    void Start()
    {
        if (listenPrimary) watcher.primaryButtonPress.AddListener(OnButtonEvent);
        if (listenSecondary) watcher.secondaryButtonPress.AddListener(OnButtonEvent);
        if (listenGrip) watcher.gripPress.AddListener(OnButtonEvent);
        if (listenTrigger) watcher.triggerPress.AddListener(OnButtonEvent);
        if (listenJoystickClick) watcher.joystickClick.AddListener(OnButtonEvent);
    }
    public void OnButtonEvent(bool pressed)
    {
        isPressed = pressed;
        OnValueChanged?.Invoke(pressed); // <--- llama a quien necesite saberlo
    }
}