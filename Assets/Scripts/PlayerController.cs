using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerController : MonoBehaviour
{
    [Header("Boost con Trigger")]
    public float maxBoostSpeed = 15f;
    public float acceleration = 5f;
    public float deceleration = 4f;

    [Header("Rotación del Avión")]
    public float pitchSpeed = 60f;   // arriba / abajo
    public float yawSpeed = 80f;     // girar izquierda / derecha
    public float autoRollAmount = 35f; // inclinación máxima al girar
    public float rollSmooth = 3f;      // velocidad con la que se ajusta el roll

    private CharacterController characterController;
    private float currentBoostSpeed = 0f;
    private Vector3 moveDirection = Vector3.zero;

    [Header("Referencia de dirección")]
    public Transform targetObject;  // define la dirección Z+

    private float yaw;
    private float pitch;
    private float roll;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleRotation();
        HandleBoost();
    }

    private void HandleRotation()
    {
        float horizontal = Input.GetAxis("Horizontal"); // joystick izquierda/derecha
        float vertical = Input.GetAxis("Vertical");     // joystick arriba/abajo

        // Yaw (giro izquierda/derecha)
        yaw += horizontal * yawSpeed * Time.deltaTime;

        // Pitch (subir/bajar)
        pitch -= vertical * pitchSpeed * Time.deltaTime;

        // Roll automático: inclinamos según el giro horizontal
        float targetRoll = -horizontal * autoRollAmount;
        roll = Mathf.Lerp(roll, targetRoll, Time.deltaTime * rollSmooth);

        // Aplicamos la rotación combinada
        Quaternion rotation = Quaternion.Euler(pitch, yaw, roll);
        transform.rotation = rotation;
    }

    private void HandleBoost()
    {
        bool isBoosting = Input.GetButton("Fire1");

        if (isBoosting)
        {
            currentBoostSpeed = Mathf.MoveTowards(currentBoostSpeed, maxBoostSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentBoostSpeed = Mathf.MoveTowards(currentBoostSpeed, 0f, deceleration * Time.deltaTime);
        }

        // Dirección de movimiento
        Vector3 forwardDir = (targetObject != null) ? targetObject.forward : transform.forward;

        moveDirection = forwardDir * currentBoostSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
