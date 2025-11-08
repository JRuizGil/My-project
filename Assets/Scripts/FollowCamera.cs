using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 5f, -15f);
    public float smoothSpeed = 5f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public bool isFollowing = false;
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    void LateUpdate()
    {
        if (isFollowing)
        {
            if (target == null) return;

            // La posición correcta relativa al avión en todas las rotaciones
            Vector3 desiredPosition = target.TransformPoint(offset);

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Mirar hacia la parte frontal del avión
            transform.LookAt(target);
        }
    }

    public void ResetCamera()
    {
        isFollowing = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}