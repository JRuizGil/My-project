using System;
using UnityEngine;

public class ChangeCameraMode : MonoBehaviour
{
    public FollowCamera FollowCamera;
    public VRPlayerController player;

    private void Start()
    {
        player.enabled = false;
    }

    public void PlayButton()
    {
        FollowCamera.enabled = true;
        FollowCamera.Changecamera();
        player.enabled = true; 
    }
    
}
