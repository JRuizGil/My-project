using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 5f, -15f);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // La posición correcta relativa al avión en todas las rotaciones
        Vector3 desiredPosition = target.TransformPoint(offset);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Mirar hacia la parte frontal del avión
        transform.LookAt(target);
    }
}