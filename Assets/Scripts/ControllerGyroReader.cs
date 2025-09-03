using UnityEngine;
using UnityEngine.XR;

public class ControllerGyroReader : MonoBehaviour
{
    private float timer = 0f;

    public XRNode controllerNode = XRNode.RightHand; // o LeftHand
    public Transform objetoARotar;

    private InputDevice controller;

    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(controllerNode);
    }

    void Update()
    {
        if (!controller.isValid)
        {
            controller = InputDevices.GetDeviceAtXRNode(controllerNode);
        }

        if (controller.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
        {
            objetoARotar.rotation = rotation;

            // Convertir Quaternion a Euler (grados)
            Vector3 euler = rotation.eulerAngles;

            // Contador de tiempo para mostrar cada segundo
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                Debug.Log($"Rotación -> X: {euler.x:F1}°, Y: {euler.y:F1}°, Z: {euler.z:F1}°");
                timer = 0f;
            }
        }
    }

}