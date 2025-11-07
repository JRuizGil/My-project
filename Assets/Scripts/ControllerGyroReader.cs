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

    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(controllerNode);
        targetRotation = objetoARotar.rotation;
    }

    void FixedUpdate()
    {
        if (!controller.isValid)
        {
            controller = InputDevices.GetDeviceAtXRNode(controllerNode);
        }

        if (controller.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
        {
            // Guardamos la rotación del control como destino
            targetRotation = rotation;

            // Suavizamos el giro del objeto
            objetoARotar.rotation = Quaternion.Slerp(
                objetoARotar.rotation,
                targetRotation,
                Time.deltaTime * smoothSpeed
            );

            // Convertir Quaternion a Euler (grados)
            Vector3 euler = objetoARotar.rotation.eulerAngles;

            // Mostrar cada segundo
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                Debug.Log($"Rotación (suavizada) -> X: {euler.x:F1}°, Y: {euler.y:F1}°, Z: {euler.z:F1}°");
                timer = 0f;
            }
        }
    }
}