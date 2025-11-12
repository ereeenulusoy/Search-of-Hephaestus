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


    private void FinishReload() => player.visual.IncreaseRigWeight(); 
    private void BringBackAiming() => player.weapon.isAimChasing =true;
}
