using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }


    private void ReturnRigs()
    {
    player.visual.IncreaseRigWeight(); 
    player.visual.IncreaseLeftHandIKWeight();
    }
    private void FinishGrab()
    {
    player.visual.SetBusyGrabbingState(false);
    }
    private void ReloadBringAim() => player.weapon.isReloadFinished = true;

    private void FinishShooting()
    {
        player.visual.IncreaseRigWeight();
        player.weapon.isShootFinished = true;
    }
}
