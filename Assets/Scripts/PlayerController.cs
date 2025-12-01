using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerController : MonoBehaviour
{
    [Header("Control de Juego")]
    public bool canControl = false; // <--- 2. Esta variable la activará el UImanager al dar Play

    [Header("Boost")]
    public float maxBoostSpeed = 15f;
    public float acceleration = 5f;
    public float deceleration = 4f;

    [Header("Rotación del Avión")]
    public float pitchSpeed = 60f;
    public float yawSpeed = 80f;

    [Header("Referencia de dirección para avanzar")]
    public Transform targetObject;

    private CharacterController characterController;
    private float currentBoostSpeed = 0f;
    
    private bool isBoosting = false;

    // Estado inicial
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public FollowCamera cameraFollow;
    public ChangeCameraMode cameraMode;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cameraFollow = FindFirstObjectByType<FollowCamera>();
        if (cameraMode == null)
            cameraMode = GetComponent<ChangeCameraMode>();
    }
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        currentBoostSpeed = 0;
        canControl = false; // Nos aseguramos de empezar bloqueados
    }
    void Update()
    {
        if (!canControl) return; 
        HandleBoost();
    }
    public void HandleBoost()
    {
        // Acelerar o frenar
        if (isBoosting)
            currentBoostSpeed = Mathf.MoveTowards(currentBoostSpeed, maxBoostSpeed, acceleration * Time.deltaTime);
        else
            currentBoostSpeed = Mathf.MoveTowards(currentBoostSpeed, 0f, deceleration * Time.deltaTime);

        // Si la velocidad es muy baja, no mover para ahorrar recursos
        if (currentBoostSpeed <= 0.01f)
            return;

        // Avanzar usando la dirección real del avión
        Vector3 forwardDir = transform.forward;
        characterController.Move(forwardDir * (currentBoostSpeed * Time.deltaTime));
    }
    public void ResetToStart()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentBoostSpeed = 0f;
        isBoosting = false;
        canControl = false; // <--- Bloqueamos el control al reiniciar
        cameraFollow.ResetCamera();
    }
    public void SetBoost(bool state)
    {
        isBoosting = state;
    }

}
