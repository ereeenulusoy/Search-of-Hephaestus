using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerVisualController : MonoBehaviour
{
    private Player player;
    
    [Header ("Rig")]
    [SerializeField] Rig aimRig;
    [SerializeField] private float aimRigIncreaseSpeed;
    private bool shouldAimRigIncreased;

    [Header("Left Hand IK")]
    [SerializeField] Transform leftHandTarget;
    [SerializeField] Rig leftHandIKRig;
    [SerializeField] float leftHandIK_IncreaseSpeed;
    private bool shouldLeftHandWeightIncreased;

    private bool busyGrabbingWeapon;

    private Animator anim;
    [SerializeField] private Transform[] gunTransform;

    [SerializeField] private Transform pistolGun;
    [SerializeField] private Transform shotgun;

    private Transform currentGun;


    private void Start()
    {
        player = GetComponent<Player>();
        SwitchOn(pistolGun);
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        SwitchWeaponVisuals();

        if (Input.GetKeyDown(KeyCode.R) && !busyGrabbingWeapon && player.weapon.isReloadFinished)
        {
            WeaponPauseRig();

            anim.SetTrigger("Reload");
            player.weapon.isReloadFinished = false;
        }

        UpdateAimRigWeight();
        UpdateLeftHandIKWeight();
    }


    private void UpdateAimRigWeight()
    {
        if (shouldAimRigIncreased)
        {
            aimRig.weight += aimRigIncreaseSpeed * Time.deltaTime;
           

            if (aimRig.weight >= 1f)
                shouldAimRigIncreased = false;
            
        }
    }
    private void UpdateLeftHandIKWeight()
    {
        if (shouldLeftHandWeightIncreased)
        {
            leftHandIKRig.weight += leftHandIK_IncreaseSpeed * Time.deltaTime;

            if (leftHandIKRig.weight >= 1f)
                shouldLeftHandWeightIncreased = false;
        }
    }

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        WeaponPauseRig();
        anim.SetFloat("WeaponGrabType", ((float)grabType));
        // Sadece Blend Tree'yi ayarladýk. Ancak animasyonu baþlatmadýk.
        anim.SetTrigger("WeaponGrab");

        SetBusyGrabbingState(true);
    }

    public void SetBusyGrabbingState(bool busy)
    {
        busyGrabbingWeapon = busy;
        anim.SetBool("BusyGrabbingWeapon", busyGrabbingWeapon);
    }

    private void WeaponPauseRig()
    {
        aimRig.weight = 0.1f;
        leftHandIKRig.weight = 0f;
    }
    public void ShootPauseRig()
    {
        aimRig.weight = 0.65f;
        leftHandIKRig.weight = 1f;
    }

    public void IncreaseRigWeight() => shouldAimRigIncreased = true;
    public void IncreaseLeftHandIKWeight() => shouldLeftHandWeightIncreased =true;
   
    private void SwitchWeaponVisuals()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistolGun);
            SwitchAnimationLayers(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayers(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }

    private void SwitchOn(Transform weaponTransform)
    {
        SwitchOffWeapons();
        weaponTransform.gameObject.SetActive(true);
        currentGun = weaponTransform;

        AttachLeftHand();
    }

    private void SwitchOffWeapons()
    {
        for (int i = 0; i < gunTransform.Length; i++)
        {
            gunTransform[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetPosition>().transform;


        leftHandTarget.localPosition = targetTransform.localPosition;
        leftHandTarget.localRotation = targetTransform.localRotation;


    }

    private void SwitchAnimationLayers(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(layerIndex, 1);
    }
}

public enum GrabType
{ 
 SideGrab,
 BackGrab
};
