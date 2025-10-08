using UnityEngine;
using UnityEngine.UI; // Necesario para el feedback visual
using TMPro;

public class PhotoTakerSimplified : MonoBehaviour
{
    // ASIGNAR EN EL INSPECTOR (¡Asegúrate de que no estén vacíos!)
    [Header("Componentes Requeridos")]
    public Transform cameraMountPoint; // Dónde está la cámara en el avión
    public PhotoTarget currentTarget; // El objetivo a copiar

    [Header("Configuración de Puntuación")]
    private const float MIN_SIMILARITY_THRESHOLD = 0.80f; // 80%

    [Header("Parámetros de Similitud (Pesos y Tolerancia)")]
    // Distribución: 50% Posición, 50% Rotación (Simplificado)
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
        public float fieldOfView; // Mantenido para futura expansión
    }
    
    void Update()
    {
        // Prueba la función principal con la tecla ESPACIO
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakePhotoAndCompare();
        }
    }

    // --- FUNCIÓN PRINCIPAL DE CAPTURA Y COMPARACIÓN ---
    public void TakePhotoAndCompare()
    {
        if (cameraMountPoint == null || currentTarget == null)
        {
            // ¡Este mensaje de error es clave para detectar referencias faltantes!
            if (feedbackText != null)
                feedbackText.text = "ERROR: ¡Asigna PhotoCamera o PhotoTarget en el Inspector!";
            Debug.LogError("Error: PhotoCamera Mount Point o Current Target no asignados.");
            return;
        }

        // 1. CAPTURA DE DATOS GEOMÉTRICOS
        PhotoData playerShot = new PhotoData
        {
            position = cameraMountPoint.position,
            rotation = cameraMountPoint.rotation,
            fieldOfView = currentTarget.fieldOfView // Asumimos que el FOV es el de la referencia para esta prueba
        };

        // 2. COMPARACIÓN
        float similarityScore = ComparePhoto(playerShot, currentTarget);
        
        // 3. RESULTADO Y FEEDBACK
        string resultMessage = $"Similitud: {similarityScore:P2} (Pos: {CalculatePositionScore(playerShot, currentTarget):P0}%, Rot: {CalculateRotationScore(playerShot, currentTarget):P0}%) \n";
        
        if (similarityScore >= MIN_SIMILARITY_THRESHOLD)
        {
            resultMessage += "¡Éxito! Foto copiada al 80% o más.";
        }
        else
        {
            resultMessage += "¡Falló! Necesitas mejor posición y ángulo.";
        }
        
        if (feedbackText != null)
            feedbackText.text = resultMessage;
        
        Debug.Log(resultMessage);
    }
    
    // --- FUNCIÓN DE CÁLCULO DE PUNTUACIÓN ---
    private float ComparePhoto(PhotoData playerShot, PhotoTarget target)
    {
        float positionScore = CalculatePositionScore(playerShot, target);
        float rotationScore = CalculateRotationScore(playerShot, target);

        // Suma ponderada
        return (positionScore * weightPosition) + (rotationScore * weightRotation);
    }

    // Calcula la puntuación de posición (0 a 1)
    private float CalculatePositionScore(PhotoData playerShot, PhotoTarget target)
    {
        float distance = Vector3.Distance(playerShot.position, target.position);
        // Clamp01 asegura que el valor está entre 0 y 1. Si distance > maxDistanceAllowed, es 0.
        return 1f - Mathf.Clamp01(distance / maxDistanceAllowed);
    }

    // Calcula la puntuación de rotación (0 a 1)
    private float CalculateRotationScore(PhotoData playerShot, PhotoTarget target)
    {
        float angleDifference = Quaternion.Angle(playerShot.rotation, target.rotation);
        // Clamp01 asegura que el valor está entre 0 y 1. Si angleDifference > maxAngleAllowed, es 0.
        return 1f - Mathf.Clamp01(angleDifference / maxAngleAllowed);
    }
}