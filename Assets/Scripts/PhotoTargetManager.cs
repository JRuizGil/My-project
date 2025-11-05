using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; // NECESARIO para referenciar el componente Image

public class PhotoTargetManager : MonoBehaviour
{
    [Header("Objetivos del Juego")]
    public List<PhotoTarget> availableTargets; 
    
    [Header("Referencias")]
    public PhotoTakerSimplified photoTaker; 
    public TMP_Text currentTargetNameText;
    // NUEVA REFERENCIA: El componente Image que muestra la foto
    public Image referencePhotoDisplay; // ¡Asigna esto en el Inspector!
    
    private int currentTargetIndex = -1;
    public float delayBeforeNextMission = 3.0f;

    void Start()
    {
        StartCoroutine(StartMissionSequence(0.5f)); 
    }

    public void MissionCompleted()
    {
        StartCoroutine(StartMissionSequence(delayBeforeNextMission));
    }

    private System.Collections.IEnumerator StartMissionSequence(float delay)
    {
        yield return new WaitForSeconds(delay);

        currentTargetIndex++;

        if (currentTargetIndex >= availableTargets.Count)
        {
            if (currentTargetNameText != null)
                currentTargetNameText.text = "¡Juego Terminado! Misión Completa.";
            
            // Opcional: Limpiar la imagen de referencia al terminar
            if (referencePhotoDisplay != null) 
                referencePhotoDisplay.sprite = null; 
            
            Debug.Log("Juego Terminado.");
            yield break;
        }

        // --- ASIGNACIÓN DEL NUEVO OBJETIVO ---
        PhotoTarget nextTarget = availableTargets[currentTargetIndex];
        
        if (photoTaker != null)
        {
            photoTaker.currentTarget = nextTarget;
            
            // 1. ACTUALIZAR LA IMAGEN DE REFERENCIA (LO NUEVO)
            if (referencePhotoDisplay != null && nextTarget.referenceImage != null)
            {
                // Crea un Sprite a partir de la Texture2D y asígnalo al componente Image
                referencePhotoDisplay.sprite = Sprite.Create(
                    nextTarget.referenceImage, 
                    new Rect(0, 0, nextTarget.referenceImage.width, nextTarget.referenceImage.height), 
                    new Vector2(0.5f, 0.5f)
                );
                referencePhotoDisplay.enabled = true; // Asegurarse de que el Image esté visible
            }
            
            // 2. Reiniciar el estado del jugador
            photoTaker.VRPlayerController.enabled = true; 
            photoTaker.timer.StartTimer(); 
            
            // 3. Dar feedback de texto
            if (currentTargetNameText != null)
                currentTargetNameText.text = $"Objetivo {currentTargetIndex + 1}/{availableTargets.Count}: {nextTarget.name}";
            
            if (photoTaker.feedbackText != null)
                photoTaker.feedbackText.text = $"¡Comienza la Misión {currentTargetIndex + 1}!{nextTarget.name} pulsa la tecla E para ver la foto nueva.";
        }
    }
}