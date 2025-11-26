using System;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    private VRPlayerController playerController;
    private FollowCamera camera;
    private PhotoTakerSimplified  phototaken;

    private void Awake()
    {
        playerController = FindAnyObjectByType<VRPlayerController>();
        camera = FindFirstObjectByType<FollowCamera>();
        phototaken = FindFirstObjectByType<PhotoTakerSimplified>();
    }

    public void PlayButton()
    {
        phototaken.phototaken = false;
        playerController.enabled = true;
        camera.isFollowing = true;
    }

    public void ExitButton()
    {
        Debug.Log("Exit");
    }

    public void PictureButton()
    {
        Debug.Log("Picture");
    }

    public void creditsButton()
    {
        Debug.Log("Credits");
    }
}
