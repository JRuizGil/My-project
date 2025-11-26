using UnityEngine;
using UnityEngine.UI; // Usado para Image
using TMPro;

public class PhotoTakerSimplified : MonoBehaviour
{
    [Header("Gestión de Misión")]
    public PhotoTargetManager targetManager; // Referencia al Manager
    
    [Header("Componentes del Avión")]
    public FollowCamera followCamera;
    public VRPlayerController VRPlayerController; // Tu script de control (probablemente el ArcadePlaneController ahora)
    
    [Header("Componentes Requeridos de Foto")]
    public Transform cameraMountPoint; // Posicion de camara en el avion
    public PhotoTarget currentTarget; // Foto que hay que copiar
    
    
    
    // --- ¡ESTA ES LA LÍNEA QUE TE FALTA! ---
    [HideInInspector] 
    public bool isMissionActive = false;
    
    [Header("Configuración de Puntuación")]
    private const float MIN_SIMILARITY_THRESHOLD = 0.80f; // 80%

    [Header("Parámetros de Similitud (Pesos y Tolerancia)")]
    [Range(0, 1)] public float weightPosition = 0.50f; 
    [Range(0, 1)] public float weightRotation = 0.50f;
    
    [Space]
    public float maxDistanceAllowed = 50f; // Tolerancia de distancia (metros)
    public float maxAngleAllowed = 15f;    // Tolerancia de ángulo (grados)
    
    [Header("Estado Interno")]
    public bool phototaken; // Bloqueador para evitar fotos múltiples

    [Header("UI")]
    public TMP_Text feedbackText;

    // Estructura de Datos (Igual que antes)
    [System.Serializable]
    public struct PhotoData
    {
        public Vector3 position;
        public Quaternion rotation;
        public float fieldOfView; 
    }

    void Start()
    {
        // (Nota: FindFirstObjectByType es lento, es mejor asignarlo en el Inspector)
        if (followCamera == null) 
            followCamera = FindFirstObjectByType<FollowCamera>();
    }

    void Update()
    {
        // Input de prueba con el teclado
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakePhotoAndCompare();
        }
    }

    /// <summary>
    /// Función principal. Llamada por el input de VR o el teclado.
    /// </summary>
    public void TakePhotoAndCompare()
    {
        // --- 1. FILTRO DE BLOQUEO ---
        // Si ya hemos procesado una foto (phototaken == true),
        // salimos de la función inmediatamente.
        // Esto detendrá la 2ª, 3ª, 4ª, 5ª y 6ª llamada del mando de VR.
        if (phototaken)
        {
            return; 
        }

        // --- 2. BLOQUEO INMEDIATO ---
        // Si la función no se detuvo, es la primera llamada.
        // La bloqueamos INMEDIATAMENTE.
        phototaken = true;

        if (currentTarget == null || targetManager == null)
        {
             if (feedbackText != null) feedbackText.text = "ERROR: Misión no configurada.";
             Debug.LogError("PhotoTarget o PhotoTargetManager no asignado.");
             phototaken = false; // Desbloquear si hay un error
             return;
        }

        // 1. CAPTURA DE DATOS GEOMÉTRICOS
        PhotoData playerShot = new PhotoData
        {
            position = cameraMountPoint.position,
            rotation = cameraMountPoint.rotation,
            fieldOfView = currentTarget.fieldOfView 
        };

        // 2. comparar
        float similarityScore = ComparePhoto(playerShot, currentTarget);
        
        // 3. feedback
        string resultMessage = $"Similitud: {similarityScore:P2} (Pos: {CalculatePositionScore(playerShot, currentTarget):P0}%, Rot: {CalculateRotationScore(playerShot, currentTarget):P0}%) \n";
        
        // --- GESTIÓN DEL ÉXITO O FRACASO ---
        if (similarityScore >= MIN_SIMILARITY_THRESHOLD)
        {
            // ÉXITO
            resultMessage += "¡Éxito! Foto copiada al 80% o más. Reiniciando para el siguiente objetivo...";
            // 'phototaken' ya es true. Se queda bloqueado.
            targetManager.MissionCompleted();
            ResetGameForNextMission();
        }
        else // Fallo
        {
            // FALLO
            resultMessage += "¡Falló! Necesitas mejor posición y ángulo.";
            
            // --- 4. DESBLOQUEAR SI FALLA ---
            // Si el jugador falla, desbloqueamos la cámara
            // para que pueda intentarlo de nuevo.
            phototaken = false;
        }
        
        if (feedbackText != null)
            feedbackText.text = resultMessage;
        
        Debug.Log(resultMessage);
    }

    /// <summary>
    /// Reinicia la posición del jugador, la cámara y el temporizador.
    /// </summary>
    private void ResetGameForNextMission()
    {
        // Resetear la posición del jugador y desactivar controles (temporalmente)
        if (VRPlayerController != null)
        {
            VRPlayerController.ResetToStart(); // Asumiendo que esta función existe en tu script
            VRPlayerController.enabled = false; // Deshabilitamos controles mientras se carga/muestra feedback
        }
    }

    // --- FUNCIONES DE CÁLCULO DE PUNTUACIÓN ---
    
    private float ComparePhoto(PhotoData playerShot, PhotoTarget target)
    {
        float positionScore = CalculatePositionScore(playerShot, target);
        float rotationScore = CalculateRotationScore(playerShot, target);
        return (positionScore * weightPosition) + (rotationScore * weightRotation);
    }
    
    private float CalculatePositionScore(PhotoData playerShot, PhotoTarget target)
    {
        float distance = Vector3.Distance(playerShot.position, target.position);
        return 1f - Mathf.Clamp01(distance / maxDistanceAllowed);
    }

    private float CalculateRotationScore(PhotoData playerShot, PhotoTarget target)
    {
        float angleDifference = Quaternion.Angle(playerShot.rotation, target.rotation);
        return 1f - Mathf.Clamp01(angleDifference / maxAngleAllowed);
    }

    // --- 5. FUNCIÓN DE DESBLOQUEO (La que te faltaba) ---
    /// <summary>
    /// Desbloquea la cámara para que el jugador pueda tomar una foto en la nueva misión.
    /// Llamada por el PhotoTargetManager.
    /// </summary>
    public void ResetPhotoLock()
    {
        phototaken = false;
    }
}