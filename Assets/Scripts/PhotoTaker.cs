using UnityEngine;
using UnityEngine.UI; // Necesario para el feedback visual
using TMPro;

public class PhotoTakerSimplified : MonoBehaviour
{
    
    [Header("Componentes Requeridos")]
    public Transform cameraMountPoint; // Posicion de camara en el avion
    public PhotoTarget currentTarget; // Foto que hay que copiar
    

    [Header("Configuración de Puntuación")]
    private const float MIN_SIMILARITY_THRESHOLD = 0.80f; // 80%

    [Header("Parámetros de Similitud (Pesos y Tolerancia)")]
    //distribucion de puntuacion 50/50
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
    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakePhotoAndCompare();
        }
    }

    //captura y comparar
    public void TakePhotoAndCompare()
    {
    

        // 1. CAPTURA DE DATOS GEOMÉTRICOS
        PhotoData playerShot = new PhotoData
        {
            position = cameraMountPoint.position,
            rotation = cameraMountPoint.rotation,
            fieldOfView = currentTarget.fieldOfView 
        };

        //comparar
        float similarityScore = ComparePhoto(playerShot, currentTarget);
        
        // feedback
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
    
    // calcular puntuacion
    private float ComparePhoto(PhotoData playerShot, PhotoTarget target)
    {
        float positionScore = CalculatePositionScore(playerShot, target);
        float rotationScore = CalculateRotationScore(playerShot, target);

        // suma
        return (positionScore * weightPosition) + (rotationScore * weightRotation);
    }

    // puatuacion de posicion de 0 a 1
    private float CalculatePositionScore(PhotoData playerShot, PhotoTarget target)
    {
        float distance = Vector3.Distance(playerShot.position, target.position);
        // Clamp01 asegura que el valor está entre 0 y 1. Si distance > maxDistanceAllowed, es 0.
        return 1f - Mathf.Clamp01(distance / maxDistanceAllowed);
    }

    // puntacion de rotacion (0 a 1)
    private float CalculateRotationScore(PhotoData playerShot, PhotoTarget target)
    {
        float angleDifference = Quaternion.Angle(playerShot.rotation, target.rotation);
        
        return 1f - Mathf.Clamp01(angleDifference / maxAngleAllowed);
    }
}