using System;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    [Header("Referencias a Paneles UI")]
    public GameObject mainMenuPanel; // Asigna aquí tu panel principal
    public GameObject creditsPanel;  // Asigna aquí tu panel de créditos
    public GameObject picturePanel;  // Asigna aquí tu panel de fotos

    [Header("Referencias de Jugador")]
    private VRPlayerController playerController;
    private FollowCamera cameraScript; // Cambié 'camera' a 'cameraScript' para evitar conflictos con nombres reservados
    private PhotoTakerSimplified phototaken;

    private void Awake()
    {
        playerController = FindAnyObjectByType<VRPlayerController>();
        cameraScript = FindFirstObjectByType<FollowCamera>();
        phototaken = FindFirstObjectByType<PhotoTakerSimplified>();
        
        // Opcional: Asegurarnos de empezar con el menú abierto y los otros cerrados
        if(mainMenuPanel != null) OpenPanel(mainMenuPanel);
    }

    public void PlayButton()
    {
        // Ocultamos el menú principal al jugar
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);

        phototaken.phototaken = false;
        
        if (playerController != null)
        {
            playerController.enabled = true; // Nos aseguramos que el script corra
            playerController.canControl = true; // Le damos permiso para volar
        }
        
        cameraScript.isFollowing = true;
    }

    public void ExitButton()
    {
        Debug.Log("Cerrando Juego");
        Application.Quit();

        // Este código extra permite que el botón también funcione mientras pruebas en el Editor de Unity
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // Abre el panel de Créditos y cierra el Menú Principal
    public void CreditsButton()
    {
        OpenPanel(creditsPanel);
    }

    // Abre el panel de Picture y cierra el Menú Principal
    public void PictureButton()
    {
        OpenPanel(picturePanel);
    }

    // Esta función se usará en los botones "Atrás" o "Cerrar" dentro de Créditos y Picture
    // para volver al Menú Principal
    public void BackToMenuButton()
    {
        OpenPanel(mainMenuPanel);
    }

    // Función auxiliar para manejar el cambio de paneles limpiamente
    private void OpenPanel(GameObject panelToOpen)
    {
        // Primero desactivamos todos para evitar superposiciones
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (picturePanel != null) picturePanel.SetActive(false);

        // Activamos solo el que queremos
        if (panelToOpen != null) panelToOpen.SetActive(true);
    }
}
