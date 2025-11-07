using UnityEngine;
using UnityEngine.XR;

public class ControllerGyroReader : MonoBehaviour
{
    private float timer = 0f;

    public XRNode controllerNode = XRNode.RightHand; // o LeftHand
    public Transform objetoARotar;
    public float smoothSpeed = 8f; // Velocidad de suavizado (ajusta según lo que quieras)

    private InputDevice controller;
    private Quaternion targetRotation; // Rotación que queremos alcanzar
    public Vector3 controllerForwardCorrection = new Vector3(0, 0, 0); // Ajustaremos aquí
    private Quaternion correction;

    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(controllerNode);
        correction = Quaternion.Euler(controllerForwardCorrection);
        targetRotation = objetoARotar.rotation;
    }

    void FixedUpdate()
    {
        HandleControllerGyro();
    }

    public void HandleControllerGyro()
    {
        if (!controller.isValid)
            controller = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (controller.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
        {
            rotation = rotation * correction;

            targetRotation = rotation;

            objetoARotar.rotation = Quaternion.Slerp(
                objetoARotar.rotation,
                targetRotation,
                Time.deltaTime * smoothSpeed
            );
        }
    }
}