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

    public void ReloadIsOver()
    {
        player.visual.IncreaseRigWeight();
        player.visual.IncreaseLeftHandIKWeight();
        player.weapon.CurrentWeapon().RefillBullets();

       
    }
    public void ReloadBringAim() => player.weapon.SetWeaponReady(true);
    // Rig + Reload BringAim.
    public void ReturnRigs()
    {
        player.visual.IncreaseRigWeight(); 
        player.visual.IncreaseLeftHandIKWeight();
    }
    public void FinishGrab()
    {
        player.weapon.SetWeaponReady(true);
    }
   
    public void SwitchOnCurrentWeaponModel() => player.visual.SwitchOnCurrentWeaponModel();
    
}
