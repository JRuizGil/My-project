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
    
    public Image referencePhotoDisplay; 
    
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
            
            
            if (referencePhotoDisplay != null) 
                referencePhotoDisplay.sprite = null; 
            
            Debug.Log("Juego Terminado.");
            yield break;
        }

       
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
            
            if (currentTargetNameText != null)
                currentTargetNameText.text = $"Objetivo {currentTargetIndex + 1}/{availableTargets.Count}: {nextTarget.name}";
            
        }
    }
}