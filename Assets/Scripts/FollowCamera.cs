using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public VRPlayerController player;
    public Vector3 offset;// relativo al avión
    public float smoothSpeed = 5f;

    // 📍 Posición y rotación inicial de la cámara
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialOffset;

    void Start()
    {
        // Guardamos la posición, rotación y offset iniciales
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialOffset = offset;
    }

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

    public void Changecamera()
    {
        offset.z = -20f;
        offset.y = 5f;
    }

    // 🔁 Método público para resetear la cámara a su posición original
    public void ResetCamera()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        offset = initialOffset;
    }
}