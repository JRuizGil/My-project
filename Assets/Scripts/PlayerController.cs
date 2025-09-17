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


    private CharacterController characterController;
    private float fallingSpeed;
    private float currentBoostSpeed = 0f; // velocidad actual al usar trigger
    private Vector3 boostDirection = Vector3.zero; // última dirección usada

    public Transform targetObject;        // Objeto cuya Z+ define la dirección
    
    public bool isAccelerating()
    {
        return true;
    }

    private void Update()
    {
        MovePlane();
        //if (Input.GetAxis("Fire1"))
        //{
        //    AcceleratePlane();
        //}
    }

    public void MovePlane()
    {
        //if(Input.GetAxis("Horizontal"))
        //{
        //    
        //}
        //if(Input.GetAxis("Vertical"))
        //{
        //    
        //}
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
