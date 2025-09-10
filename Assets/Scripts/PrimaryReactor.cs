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

    [Header("Animación de rotación")]
    public Vector3 rotationAngle = new Vector3(45, 45, 45);
    public float rotationDuration = 0.25f; // segundos

    private Quaternion _offRotation;
    private Quaternion _onRotation;
    private Coroutine _rotator;

    void Start()
    {
        // Guardamos rotaciones base
        _offRotation = this.transform.rotation;
        _onRotation = Quaternion.Euler(rotationAngle) * _offRotation;

        // Nos suscribimos solo a los eventos que quieras usar
        if (listenPrimary) watcher.primaryButtonPress.AddListener(OnButtonEvent);
        if (listenSecondary) watcher.secondaryButtonPress.AddListener(OnButtonEvent);
        if (listenGrip) watcher.gripPress.AddListener(OnButtonEvent);
        if (listenTrigger) watcher.triggerPress.AddListener(OnButtonEvent);
        if (listenJoystickClick) watcher.joystickClick.AddListener(OnButtonEvent);
    }

    public void OnButtonEvent(bool pressed)
    {
        isPressed = pressed;
        if (_rotator != null)
            StopCoroutine(_rotator);

        if (pressed)
            _rotator = StartCoroutine(ButtonAction(transform.rotation, _onRotation));
        else
            _rotator = StartCoroutine(ButtonAction(transform.rotation, _offRotation));
    }

    private IEnumerator ButtonAction(Quaternion fromRotation, Quaternion toRotation)
    {
        float t = 0;
        while (t < rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, t / rotationDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = toRotation; // rotar
    }
}
