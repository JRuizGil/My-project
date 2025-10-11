using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonReactor : MonoBehaviour
{
    [Header("Referencia al watcher de Quest 2")]
    public Quest2InputWatcher watcher;

    [Header("Selecciona qué botón escuchar")]
    public bool listenPrimary;
    public bool listenSecondary;
    public bool listenGrip;
    public bool listenTrigger;
    public bool listenJoystickClick;

    [FormerlySerializedAs("IsPressed")] [Header("Estado (debug en Inspector)")]
    public bool isPressed;

    private Quaternion _offRotation;
    private Quaternion _onRotation;
    private Coroutine _rotator;

    void Start()
    {
        // Guardamos rotaciones base
        _offRotation = this.transform.rotation;

        // Nos suscribimos solo a los eventos que quieras usar
        if (listenPrimary) watcher.primaryButtonPress.AddListener(OnButtonEvent);
        if (listenSecondary) watcher.secondaryButtonPress.AddListener(OnButtonEvent);
        if (listenGrip) watcher.gripPress.AddListener(OnButtonEvent);
        if (listenTrigger) watcher.triggerPress.AddListener(OnButtonEvent);
        if (listenJoystickClick) watcher.joystickClick.AddListener(OnButtonEvent);
    }

    public void OnButtonEvent(bool pressed)
    {
        
    }

}
