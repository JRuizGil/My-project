using UnityEngine;
using System.Collections.Generic;
using TMPro; // Asegúrate de tener TMPro
using UnityEngine.UI; // Asegúrate de tener UI para la Image

public class PhotoTargetManager : MonoBehaviour
{
    [Header("Objetivos del Juego")]
    public List<PhotoTarget> availableTargets; // Lista de todas las fotos a tomar
    
    [Header("Referencias")]
    public PhotoTakerSimplified photoTaker; // Tu script del avión
    public TMP_Text currentTargetNameText; // Texto UI para el nombre del objetivo
    public Image referencePhotoDisplay; // Image UI para mostrar la foto objetivo
    
    // (Opcional) Añade la referencia a tu script de cámara si la tienes
    // public ChangeCameraMode cameraModeChanger; 

    private int currentTargetIndex = -1; // -1 indica que no se ha cargado el primero
    public float delayBeforeNextMission = 3.0f; // Tiempo para que el jugador vea el feedback

    void Start()
    {
        StartCoroutine(StartMissionSequence(0.5f)); 
    }

    /// <summary>
    /// Llamado por PhotoTakerSimplified cuando una foto tiene éxito.
    /// </summary>
    public void MissionCompleted()
    {
        StartCoroutine(StartMissionSequence(delayBeforeNextMission));
    }

    /// <summary>
    /// Corutina para manejar el delay entre misiones y cargar el siguiente objetivo.
    /// </summary>
    private System.Collections.IEnumerator StartMissionSequence(float delay)
    {
        yield return new WaitForSeconds(delay); // Esperar el tiempo de feedback

        currentTargetIndex++;

        // 1. Verificar si quedan objetivos (Fin del Juego)
        if (currentTargetIndex >= availableTargets.Count)
        {
            if (currentTargetNameText != null)
                currentTargetNameText.text = "¡Juego Terminado! Misión Completa.";
            
            if (photoTaker.feedbackText != null)
                photoTaker.feedbackText.text = "¡Has completado todas las misiones!";

            if (referencePhotoDisplay != null) 
                referencePhotoDisplay.enabled = false; // Ocultar la foto
            
            Debug.Log("Juego Terminado.");
            yield break; // Detener la corutina
        }

        // 2. Asignar el nuevo objetivo al PhotoTaker
        PhotoTarget nextTarget = availableTargets[currentTargetIndex];
        
        if (photoTaker != null)
        {
            photoTaker.currentTarget = nextTarget;
            
            // 3. Actualizar la UI de la Foto Objetivo
            if (referencePhotoDisplay != null && nextTarget.referenceImage != null)
            {
                referencePhotoDisplay.sprite = Sprite.Create(
                    nextTarget.referenceImage, 
                    new Rect(0, 0, nextTarget.referenceImage.width, nextTarget.referenceImage.height), 
                    new Vector2(0.5f, 0.5f)
                );
                referencePhotoDisplay.enabled = true;
            }
            
            // 4. (Opcional) Forzar la vista de cámara (si está asignado)
            // if (cameraModeChanger != null)
            // {
            //     cameraModeChanger.SetThirdPersonView(); 
            // }

            // --- ESTAS SON LAS LÍNEAS QUE FALTAN ---
            
            // 5. Reiniciar el estado del jugador para el NUEVO objetivo
            photoTaker.VRPlayerController.enabled = true; // ¡Habilitar controles!
            // photoTaker.timer.StartTimer(); // Descomenta si tienes un timer
            
            // 6. ¡DESBLOQUEAR LA CÁMARA! (Paso Crítico)
            photoTaker.ResetPhotoLock(); 
            
            // --- FIN DE LÍNEAS QUE FALTAN ---

            // 7. Dar feedback de texto al jugador
            if (currentTargetNameText != null)
                currentTargetNameText.text = $"Objetivo {currentTargetIndex + 1}/{availableTargets.Count}: {nextTarget.name}";
            
            if (photoTaker.feedbackText != null)
                photoTaker.feedbackText.text = $"¡Comienza la Misión {currentTargetIndex + 1}! Encuentra {nextTarget.name}.";
        }
    }
}