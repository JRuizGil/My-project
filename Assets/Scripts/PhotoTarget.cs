using UnityEngine;

[CreateAssetMenu(fileName = "NewPhotoTarget", menuName = "Photo Game/Photo Target")]
public class PhotoTarget : ScriptableObject
{
    [Header("Referencia Visual")]
    public Texture2D referenceImage; // La foto que se muestra al jugador

    [Header("Datos de Posición y Ángulo (La 'Foto Perfecta')")]
    public Vector3 position;
    public Quaternion rotation;
    
    [Header("Zoom de la Cámara")]
    public float fieldOfView = 60f; // El FOV con el que se tomó la referencia.
        
   
   
        
    // --- NUEVO: LA HISTORIA ---
    [Header("Narrativa")]
    public string missionName; // Ej: "La Vieja Torre"
        
    [TextArea(5, 10)] // Esto crea una caja grande en el inspector para escribir cómodo
    public string photoStory; // Ej: "Esta torre fue construida en 1890..."
}
    
    
   
