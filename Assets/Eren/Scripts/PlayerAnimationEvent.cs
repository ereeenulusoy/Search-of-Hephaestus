using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    PlayerVisualController playerVisualController;
    void Start()
    {
        playerVisualController = GetComponentInParent<PlayerVisualController>();
    }


    private void FinishReload() => playerVisualController.IncreaseRigWeight(); 
}
