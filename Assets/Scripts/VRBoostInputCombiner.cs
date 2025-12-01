using System;
using UnityEngine;

public class VRBoostInputCombiner : MonoBehaviour
{
    public VRPlayerController player;

    // Estados individuales
    private bool triggerHeld = false;
    private bool gripHeld = false;

    private void Start()
    {
        player = GetComponent<VRPlayerController>();
    }

    // Se llama cuando cambia el trigger
    public void SetTrigger(bool state)
    {
        triggerHeld = state;
        UpdateBoost();
    }

    // Se llama cuando cambia el grip
    public void SetGrip(bool state)
    {
        gripHeld = state;
        UpdateBoost();
    }

    private void UpdateBoost()
    {
        bool boosting = triggerHeld || gripHeld; // <--- si alguno está pulsado
        player.SetBoost(boosting);
    }
}