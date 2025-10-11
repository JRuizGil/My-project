using System;
using UnityEngine;

public class ChangeCameraMode : MonoBehaviour
{
    public FollowCamera FollowCamera;
    public VRPlayerController player;
    public Timer timer;

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
    }
    
}
