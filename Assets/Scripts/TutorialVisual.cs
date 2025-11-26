using UnityEngine;

public class TutorialVisual : MonoBehaviour
{
    [Header("Prefabs Visuales")]
    public GameObject ghostPlanePrefab; // El avión fantasma
    public GameObject zoneSpherePrefab; // La esfera de zona

    private GameObject currentGhost;
    private GameObject currentZone;

    // Esta función muestra los visuales en la posición del objetivo
    public void ShowTutorialVisuals(PhotoTarget target, float distanceThreshold)
    {
        HideTutorialVisuals(); // Limpiar anteriores por si acaso

        if (target == null) return;

        // 1. Crear el Avión Fantasma
        if (ghostPlanePrefab != null)
        {
            currentGhost = Instantiate(ghostPlanePrefab, target.position, target.rotation);
        }

        // 2. Crear la Esfera de Zona
        if (zoneSpherePrefab != null)
        {
            currentZone = Instantiate(zoneSpherePrefab, target.position, Quaternion.identity);
            
            // Escalamos la esfera según la distancia permitida (Radio * 2 = Diámetro)
            float diameter = distanceThreshold * 2f; 
            currentZone.transform.localScale = new Vector3(diameter, diameter, diameter);
        }
    }

    // Esta función borra los visuales
    public void HideTutorialVisuals()
    {
        if (currentGhost != null) Destroy(currentGhost);
        if (currentZone != null) Destroy(currentZone);
    }
}
