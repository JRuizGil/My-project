using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRPlayerController : MonoBehaviour
{
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
    private Vector3 moveDirection = Vector3.zero;

    private float yaw;
    private float pitch;
    private float roll;

    private bool isBoosting = false;

    // Estado inicial
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public ChangeCameraMode cameraMode;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (cameraMode == null)
            cameraMode = GetComponent<ChangeCameraMode>();
    }

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        HandleInput();
        HandleRotation();
        HandleBoost();
    }

    void HandleInput()
    {
        // Se activa mientras el botón está PRESIONADO, no solo el frame inicial
        isBoosting = Input.GetButton("Fire1");
    }

    void HandleRotation()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        yaw += horizontal * yawSpeed * Time.deltaTime;
        pitch -= vertical * pitchSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void HandleBoost()
    {
        if (isBoosting)
            currentBoostSpeed = Mathf.MoveTowards(currentBoostSpeed, maxBoostSpeed, acceleration * Time.deltaTime);
        else
            currentBoostSpeed = Mathf.MoveTowards(currentBoostSpeed, 0f, deceleration * Time.deltaTime);

        // Si no tiene velocidad, NO MOVER
        if (currentBoostSpeed <= 0.01f)
            return;

        Vector3 forwardDir = (targetObject != null) ? targetObject.forward : transform.forward;
        moveDirection = forwardDir * currentBoostSpeed;

        characterController.Move(moveDirection * Time.deltaTime);
    }


    public void ResetToStart()
    {
        cameraMode.ChangeView();

        characterController.enabled = false;

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        currentBoostSpeed = 0f;
        moveDirection = Vector3.zero;
        yaw = pitch = roll = 0f;

        characterController.enabled = true;
    }
}
