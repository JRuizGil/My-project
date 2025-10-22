using System;
using UnityEngine;

public class ChangeCameraMode : MonoBehaviour
{
    public FollowCamera FollowCamera;
    public VRPlayerController player;
    public Timer timer;
    public bool isplaying;
    public GameObject FirstpersonCamera;
    public GameObject ThirdpersonCamera;

    private void Start()
    {
        player.enabled = false;
        timer.RestartTimer();
    }

    public void PlayButton()
    {
        FollowCamera.enabled = true;
        FollowCamera.Changecamera();
        player.enabled = true; 
        timer.StartTimer();
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
