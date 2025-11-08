using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // El avión
    public Vector3 offset = new Vector3(0f, 5f, -15f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Posición con offset relativo a la rotación del avión
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // Movimiento suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Mirar hacia la dirección del avión
        transform.LookAt(target.position + target.forward * 10f);
    }
}