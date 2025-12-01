using System;
using UnityEngine;

public class ChangeCameraMode : MonoBehaviour
{
    public FollowCamera FollowCamera;
    public VRPlayerController player;
    public bool isplaying;
    public GameObject FirstpersonCamera;
    public GameObject ThirdpersonCamera;

    private void Start()
    {
        // ¡QUITAR! Esto activaba el control del jugador al inicio del juego.
        // player.enabled = true; 
        
        // El playerController debe tener canControl = false en su Start()
        if (player != null)
        {
            player.enabled = true; // El script debe estar activo para escuchar inputs y la lógica de canControl
            player.canControl = false; // Bloqueado por defecto
        }
    }

    public void PlayButton()
    {
        FollowCamera.enabled = true;
        
        // ¡ESTA ES LA CLAVE! Activamos el permiso de control (canControl)
        if (player != null)
        {
            player.enabled = true; // Aseguramos que el script esté corriendo
            player.canControl = true; // El avión ya puede volar y rotar si se aprieta el gatillo
        }
        
        isplaying = true;
    }

    public void Update()
    {
        ChangeView();
   }

    public void ChangeView()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool thirdPersonActive = ThirdpersonCamera.activeSelf;
            ThirdpersonCamera.SetActive(!thirdPersonActive);
            FirstpersonCamera.SetActive(thirdPersonActive);
        }
    }
}
