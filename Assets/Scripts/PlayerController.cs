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
    // velocidad con la que se ajusta el roll

    private CharacterController characterController;
    private float currentBoostSpeed = 0f;
    private Vector3 moveDirection = Vector3.zero;

    [Header("Referencia de dirección")]
    public Transform targetObject;  // define la dirección Z+

    private float yaw;
    private float pitch;
    private float roll;

    // 📍 Posición y rotación inicial
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isboosting = false;
    public ChangeCameraMode cameraMode;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        isboosting = Input.GetButton("Fire1");
        characterController = GetComponent<CharacterController>();

        // Guardar posición y rotación iniciales
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    private void Awake()
    {
        if (cameraMode == null)
            cameraMode = GetComponent<ChangeCameraMode>(); // fallback automático
    }

    private void Update()
    {
        HandleRotation();
        isBoosting();
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
        //float targetRoll = -horizontal * autoRollAmount;
        //roll = Mathf.Lerp(roll, targetRoll, Time.deltaTime * rollSmooth);

        // Aplicamos la rotación combinada
        Quaternion rotation = Quaternion.Euler(pitch, yaw, roll);
        transform.rotation = rotation;
    }

    public void isBoosting()
    {
        if(Input.GetKeyDown("Fire1"))
        {
            isboosting = true;
            HandleBoost();
        }
        else
        {
            isboosting = false;
        }
    }
    private void HandleBoost()
    {
        if (isboosting)
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

    // 🔁 Método público para reiniciar la posición y rotación
    public void ResetToStart()
    {
        cameraMode.ChangeView();
        // Desactivar momentáneamente el CharacterController para mover sin problemas
        
        characterController.enabled = false;

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Reiniciar variables de movimiento y rotación
        currentBoostSpeed = 0f;
        moveDirection = Vector3.zero;
        yaw = pitch = roll = 0f;

        // Volver a activar el CharacterController
        characterController.enabled = true;
        
        
    }
}
