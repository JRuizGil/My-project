using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerController : MonoBehaviour
{
    
    [Header("Boost con Trigger")]
    public float maxBoostSpeed = 6f;          // Velocidad máxima con trigger
    public float acceleration = 5f;           // Qué tan rápido acelera
    public float deceleration = 4f;           // Qué tan rápido frena al soltar

    public float rotationSpeed = 100f;

    private CharacterController characterController;
    private float fallingSpeed;
    private float currentBoostSpeed = 0f; // velocidad actual al usar trigger
    private Vector3 boostDirection = Vector3.zero; // última dirección usada
    private Rigidbody rb;
    public Transform targetObject;        // Objeto cuya Z+ define la dirección
    
    public bool isAccelerating()
    {
        return true;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        RotatePlane();
        if (Input.GetButton("Fire1")) // Fire1 devuelve bool (click/joystick button)
        {
            AcceleratePlane();
        }
    }

    public void RotatePlane()
    {
        float horizontal = Input.GetAxis("Horizontal"); // joystick izquierda/derecha
        float vertical = Input.GetAxis("Vertical");     // joystick arriba/abajo

        // Rotamos el avión (pitch y yaw)
        transform.Rotate(Vector3.forward * -horizontal * rotationSpeed * Time.deltaTime, Space.Self);     // yaw (izq-der)
        transform.Rotate(Vector3.right * vertical * rotationSpeed * Time.deltaTime, Space.Self);   // pitch (arriba-abajo)
    }
    public void AcceleratePlane()
    {
        if (isAccelerating())
        {
            // Acelera progresivamente hasta el máximo
            currentBoostSpeed = Mathf.MoveTowards(currentBoostSpeed, maxBoostSpeed, acceleration * Time.deltaTime);

            // Dirección siempre hacia el eje Z local del objeto asignado
            if (targetObject != null)
            {
                boostDirection = targetObject.forward;
                boostDirection.Normalize();
            }
        }
        else
        {
            // Si no está activo, desaceleramos progresivamente hasta 0
            currentBoostSpeed = Mathf.MoveTowards(currentBoostSpeed, 0, deceleration * Time.deltaTime);
        }
        // Aplicamos movimiento si hay velocidad
        if (currentBoostSpeed > 0.01f)
        {
            characterController.Move(boostDirection * currentBoostSpeed * Time.deltaTime);
        }
    }

}
