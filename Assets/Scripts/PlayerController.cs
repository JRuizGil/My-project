using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 2.0f;          // Velocidad normal con joystick
    public float gravity = -9.81f;
    public float additionalHeight = 0.2f;

    [Header("Boost con Trigger")]
    public InputActionProperty triggerAction; // Gatillo del mando
    public float maxBoostSpeed = 6f;          // Velocidad máxima con trigger
    public float acceleration = 5f;           // Qué tan rápido acelera
    public float deceleration = 4f;           // Qué tan rápido frena al soltar

    [Header("Input Actions (de Input System)")]
    public InputActionProperty moveAction;      // Joystick izquierdo
    public InputActionProperty teleportAction;  // Botón B (no usado ahora)

    private CharacterController characterController;
    private Transform xrHead; // Cámara VR (cabeza)
    private float fallingSpeed;
    private float currentBoostSpeed = 0f; // velocidad actual al usar trigger
    private Vector3 boostDirection = Vector3.zero; // última dirección usada

    public bool isAccelerating = false;   // Actívalo desde otro script o el Inspector
    public Transform targetObject;        // Objeto cuya Z+ define la dirección
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        xrHead = Camera.main.transform; // La cámara VR principal
    }

    void Update()
    {
        // --- Movimiento con joystick (dirección relativa al head) ---
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 forward = xrHead.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = xrHead.right;
        right.y = 0;
        right.Normalize();

        Vector3 direction = forward * input.y + right * input.x;
        characterController.Move(direction * moveSpeed * Time.deltaTime);

        AcceleratePlane();

        // Aplicar boost (si hay velocidad)
        if (currentBoostSpeed > 0.01f)
            characterController.Move(boostDirection * currentBoostSpeed * Time.deltaTime);

        // --- Gravedad ---
        if (characterController.isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.deltaTime;

        characterController.Move(Vector3.up * fallingSpeed * Time.deltaTime);
    }
    

    public void AcceleratePlane()
    {
        if (isAccelerating)
        {
            // Acelera progresivamente hasta el máximo
            currentBoostSpeed = Mathf.MoveTowards(
                currentBoostSpeed, 
                maxBoostSpeed, 
                acceleration * Time.deltaTime
            );

            // Dirección siempre hacia el eje Z local del objeto asignado
            if (targetObject != null)
            {
                boostDirection = targetObject.forward;
                boostDirection.y = 0; // si quieres ignorar inclinación vertical
                boostDirection.Normalize();
            }
        }
        else
        {
            // Si no está activo, desaceleramos progresivamente hasta 0
            currentBoostSpeed = Mathf.MoveTowards(
                currentBoostSpeed, 
                0, 
                deceleration * Time.deltaTime
            );
        }

        // Aplicamos movimiento si hay velocidad
        if (currentBoostSpeed > 0.01f)
        {
            characterController.Move(boostDirection * currentBoostSpeed * Time.deltaTime);
        }
    }

}
