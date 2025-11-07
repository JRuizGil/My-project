using UnityEngine;
using UnityEngine.UI; // Usado para Image, aunque ya usas TMPro
using TMPro;

public class PhotoTakerSimplified : MonoBehaviour
{
    // AÑADE ESTA REFERENCIA PARA GESTIONAR EL CAMBIO DE FOTO
    [Header("Gestión de Misión")]
    public PhotoTargetManager targetManager; // ¡Referencia al Manager de la lista de fotos!
    
    // Tus referencias existentes
    public FollowCamera followCamera;
    public VRPlayerController VRPlayerController;
    
    [Header("Componentes Requeridos")]
    public Transform cameraMountPoint; // Posicion de camara en el avion
    public PhotoTarget currentTarget; // Foto que hay que copiar
    

    [Header("Configuración de Puntuación")]
    private const float MIN_SIMILARITY_THRESHOLD = 0.80f; // 80%

    [Header("Parámetros de Similitud (Pesos y Tolerancia)")]
    // Distribución de puntuacion 50/50
    [Range(0, 1)] public float weightPosition = 0.50f; 
    [Range(0, 1)] public float weightRotation = 0.50f;
    
    [Space] // Espacio visual en el Inspector
    public float maxDistanceAllowed = 50f; // Tolerancia de distancia (metros)
    public float maxAngleAllowed = 15f;    // Tolerancia de ángulo (grados)

    // REFERENCIA UI SIMPLE
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
        // Asegúrate de que el juego empiece en el estado inicial correcto
        VRPlayerController.enabled = true; // El jugador puede moverse al inicio
    }

    void Update()
    {
        // **Recordatorio: Cambia esto por la entrada VR (OVRInput o Input Action Unity Event)**
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakePhotoAndCompare();
        }
    }

    // captura y comparar
    public void TakePhotoAndCompare()
    {
        if (currentTarget == null || targetManager == null)
        {
             if (feedbackText != null) feedbackText.text = "ERROR: Misión no configurada.";
             Debug.LogError("PhotoTarget o PhotoTargetManager no asignado.");
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
            resultMessage += "¡Éxito! Foto copiada al 80% o más. Reiniciando para el siguiente objetivo...";
            
            // Llamar al Manager para avanzar
            targetManager.MissionCompleted(); // Usaremos este método en el Manager
            
            // Reiniciar los componentes para la siguiente ronda/misión
            ResetGameForNextMission();

        }
        else // Fallo
        {
            resultMessage += "¡Falló! Necesitas mejor posición y ángulo.";
            
            // Simplemente reportar el fallo, pero no avanzar de misión.
            // Los componentes se dejan como están (el jugador sigue volando)
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
        // 1. Detener el timer (si es que no se detiene al inicio del Manager)
        
        // 2. Resetear la posición del jugador y desactivar controles (temporalmente)
        VRPlayerController.ResetToStart();
        VRPlayerController.enabled = false; // Deshabilitamos controles mientras se carga/muestra feedback
        
        // 3. Resetear la cámara
        followCamera.ResetCamera();
        
        // 4. Iniciar Corutina o llamar al Manager para que decida cuándo volver a habilitar los controles
        // Aquí el Manager debería tomar el control.
    }


    // El resto de las funciones de puntuación se mantienen iguales
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
}