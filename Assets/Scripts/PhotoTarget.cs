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
}