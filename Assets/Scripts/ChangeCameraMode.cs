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
        player.enabled = false;
    }

    public void PlayButton()
    {
        FollowCamera.enabled = true;
        player.enabled = true; 
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
