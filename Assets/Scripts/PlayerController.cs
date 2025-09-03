using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 2.0f;
    public float gravity = -9.81f;
    public float additionalHeight = 0.2f;

    [Header("Input Actions (de Input System)")]
    public InputActionProperty moveAction;  // Joystick izquierdo
    public InputActionProperty teleportAction; // Botón B

    private CharacterController characterController;
    private Transform xrHead; // Cámara VR (cabeza)
    private float fallingSpeed;

    [Header("Teleport")]
    public float teleportDistance = 5f; // Distancia del salto
    public LayerMask teleportLayers;    // Capas válidas para teletransporte

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        xrHead = Camera.main.transform; // La cámara VR principal
    }

    void Update()
    {
        // --- Ajustar altura del capsule al jugador ---
        UpdateCharacterHeight();

        // --- Movimiento hacia donde mira la cabeza ---
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 forward = xrHead.forward;
        forward.y = 0; // Solo en plano XZ
        forward.Normalize();

        Vector3 right = xrHead.right;
        right.y = 0;
        right.Normalize();

        Vector3 direction = forward * input.y + right * input.x;
        characterController.Move(direction * moveSpeed * Time.deltaTime);

        // --- Gravedad ---
        if (characterController.isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.deltaTime;

        characterController.Move(Vector3.up * fallingSpeed * Time.deltaTime);

        // --- Teletransporte con botón B ---
        //if (teleportAction.action.WasPressedThisFrame())
        //{
        //    //TryTeleport();
        //}
    }

    //void TryTeleport()
    //{
    //    Vector3 origin = xrHead.position;
    //    Vector3 direction = xrHead.forward;
//
    //    // Raycast hacia adelante desde la cabeza
    //    if (Physics.Raycast(origin, direction, out RaycastHit hit, teleportDistance, teleportLayers))
    //    {
    //        // Teletransportar al punto de impacto
    //        Vector3 targetPos = hit.point;
    //        targetPos.y += characterController.height / 2; // ajustar altura
    //        characterController.enabled = false;
    //        transform.position = targetPos;
    //        characterController.enabled = true;
    //    }
    //    else
    //    {
    //        // Si no golpea nada, teletransportar a una distancia fija hacia adelante
    //        Vector3 targetPos = origin + direction * teleportDistance;
    //        targetPos.y = transform.position.y; // mantener altura del suelo
    //        characterController.enabled = false;
    //        transform.position = targetPos;
    //        characterController.enabled = true;
    //    }
    //}

    void UpdateCharacterHeight()
    {
        float headHeight = Mathf.Clamp(xrHead.localPosition.y, 1, 2);
        characterController.height = headHeight + additionalHeight;

        Vector3 capsuleCenter = transform.InverseTransformPoint(xrHead.position);
        characterController.center = new Vector3(capsuleCenter.x, characterController.height / 2 + characterController.skinWidth, capsuleCenter.z);
    }
}
