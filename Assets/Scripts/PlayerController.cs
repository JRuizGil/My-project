using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 2.0f;          // Velocidad normal con joystick
    public float gravity = -9.81f;
    public float additionalHeight = 0.2f;

    [Header("Sprint con Trigger")]
    public InputActionProperty triggerAction; // Gatillo del mando
    public float maxBoostSpeed = 6f;          // Velocidad máxima con trigger
    public float acceleration = 5f;           // Qué tan rápido acelera

    [Header("Input Actions (de Input System)")]
    public InputActionProperty moveAction;      // Joystick izquierdo
    public InputActionProperty teleportAction;  // Botón B (no usado ahora)

    private CharacterController characterController;
    private Transform xrHead; // Cámara VR (cabeza)
    private float fallingSpeed;
    private float currentBoostSpeed = 0f; // velocidad actual al usar trigger

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        xrHead = Camera.main.transform; // La cámara VR principal
    }

    void Update()
    {
        // --- Ajustar altura del capsule al jugador ---
        UpdateCharacterHeight();

        // --- Movimiento con joystick ---
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 forward = xrHead.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = xrHead.right;
        right.y = 0;
        right.Normalize();

        Vector3 direction = forward * input.y + right * input.x;
        characterController.Move(direction * moveSpeed * Time.deltaTime);

        // --- Boost con Trigger ---
        float triggerValue = triggerAction.action.ReadValue<float>(); // valor 0-1
        if (triggerValue > 0.1f) // si se está presionando
        {
            currentBoostSpeed = Mathf.MoveTowards(currentBoostSpeed, maxBoostSpeed, acceleration * Time.deltaTime);
            Vector3 boostDir = xrHead.forward;
            boostDir.y = 0;
            boostDir.Normalize();
            characterController.Move(boostDir * currentBoostSpeed * Time.deltaTime);
        }
        else
        {
            currentBoostSpeed = 0; // reseteamos al soltar
        }

        // --- Gravedad ---
        if (characterController.isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.deltaTime;

        characterController.Move(Vector3.up * fallingSpeed * Time.deltaTime);
    }

    void UpdateCharacterHeight()
    {
        float headHeight = Mathf.Clamp(xrHead.localPosition.y, 1, 2);
        characterController.height = headHeight + additionalHeight;

        Vector3 capsuleCenter = transform.InverseTransformPoint(xrHead.position);
        characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2 + characterController.skinWidth, capsuleCenter.z);
    }
}
