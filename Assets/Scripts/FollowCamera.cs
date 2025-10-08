using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public VRPlayerController player;
    public Vector3 offset = new Vector3(0, 5, -10); // relativo al avión
    public float smoothSpeed = 5f;

    private void Start()
    {
        offset = new Vector3(0, -2.5f, -7.5f);
        player.enabled = false;
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
        player.enabled = true;
    }
}