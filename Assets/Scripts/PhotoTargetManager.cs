using UnityEngine;
using System.Collections.Generic;
using TMPro; 
using UnityEngine.UI; 

public class PhotoTargetManager : MonoBehaviour
{
    [Header("Objetivos del Juego")]
    public List<PhotoTarget> availableTargets; 
   
    [Header("Panel de Historia VR")]
     
    public TMP_Text storyBodyText;  
   
    [Header("Referencias")]
    public PhotoTakerSimplified photoTaker; 
    public TMP_Text currentTargetNameText; 
    public Image referencePhotoDisplay; 
    
    [Header("Tutorial")]
    public TutorialVisual tutorialVisuals; // Referencia al script del fantasma

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
        // 1. LIMPIEZA DE TUTORIAL ANTERIOR
        if (tutorialVisuals != null)
        {
            tutorialVisuals.HideTutorialVisuals();
        }

        yield return new WaitForSeconds(delay); 

        currentTargetIndex++;

        // 2. Verificar Fin del Juego
        if (currentTargetIndex >= availableTargets.Count)
        {
            if (currentTargetNameText != null)
                currentTargetNameText.text = "¡Juego Terminado! Misión Completa.";
            
            if (photoTaker.feedbackText != null)
                photoTaker.feedbackText.text = "¡Has completado todas las misiones!";

            if (referencePhotoDisplay != null) 
                referencePhotoDisplay.enabled = false; 
            
            Debug.Log("Juego Terminado.");
            yield break; 
        }

        // 3. Asignar el nuevo objetivo
        PhotoTarget nextTarget = availableTargets[currentTargetIndex];
        
        if (photoTaker != null)
        {
            photoTaker.currentTarget = nextTarget;
            
            // Actualizar UI Foto
            if (referencePhotoDisplay != null && nextTarget.referenceImage != null)
            {
                referencePhotoDisplay.sprite = Sprite.Create(
                    nextTarget.referenceImage, 
                    new Rect(0, 0, nextTarget.referenceImage.width, nextTarget.referenceImage.height), 
                    new Vector2(0.5f, 0.5f)
                );
                referencePhotoDisplay.enabled = true;
            }
            
         
            
            if (storyBodyText != null) 
                storyBodyText.text = nextTarget.photoStory;
            
            // Reiniciar estado jugador
            if (photoTaker.VRPlayerController != null) 
                photoTaker.VRPlayerController.enabled = true; 
            
            // Desbloquear cámara
            photoTaker.isMissionActive = true; 

            // --- AQUÍ ESTÁ LA CORRECCIÓN ---
            // Solo mostramos tutorial en la primera misión (índice 0)
            if (currentTargetIndex == 0 && tutorialVisuals != null)
            {
                // ¡AHORA PASAMOS LOS 2 ARGUMENTOS!
                tutorialVisuals.ShowTutorialVisuals(nextTarget, photoTaker.maxDistanceAllowed);
            }
            // -------------------------------

            // Feedback texto
            if (currentTargetNameText != null)
                currentTargetNameText.text = $"Objetivo {currentTargetIndex + 1}/{availableTargets.Count}: {nextTarget.name}";
            
            if (photoTaker.feedbackText != null)
                photoTaker.feedbackText.text = $"¡Comienza la Misión {currentTargetIndex + 1}! Encuentra {nextTarget.name}.";
        }
    }
}