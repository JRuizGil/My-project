using UnityEngine;
using UnityEngine.XR;

public class ControllerGyroReader : MonoBehaviour
{
    public bool allowGyro = false;   // Activado por ButtonReactor

    private float timer = 0f;
    VRPlayerController playercontroller;
    public XRNode controllerNode = XRNode.RightHand;
    public Transform objetoARotar;
    public float smoothSpeed = 8f;

    private InputDevice controller;
    private Quaternion targetRotation;
    public Vector3 controllerForwardCorrection = new Vector3(0, 0, 0);
    private Quaternion correction;

    void Start()
    {
        playercontroller = FindObjectOfType<VRPlayerController>();
        controller = InputDevices.GetDeviceAtXRNode(controllerNode);
        correction = Quaternion.Euler(controllerForwardCorrection);
        targetRotation = objetoARotar.rotation;
    }

    void FixedUpdate()
    {
        if (allowGyro)
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
    public void SetGyro(bool pressed)
    {
        allowGyro = pressed;
    }
}
