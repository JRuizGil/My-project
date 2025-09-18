using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    
    public Vector3 offset = new Vector3(0, 5, -10); // relativo al avión
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Offset relativo al avión
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // Interpolación suave
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Mirar hacia adelante en la dirección de la avioneta
        transform.LookAt(target.position + target.forward * 10f);
    }
}