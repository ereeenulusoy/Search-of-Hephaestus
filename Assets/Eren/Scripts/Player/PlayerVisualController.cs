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


    private Animator anim;

    [SerializeField] private WeaponModel[] weaponModels;

    


    private void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
    }
    private void Update()
    {
       
        UpdateAimRigWeight();
        UpdateLeftHandIKWeight();
    }

    public WeaponModel CurrentWeaponModel()
    {
        WeaponModel weaponModel = null;
       
        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;
        
        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i].weaponType == weaponType)
                weaponModel= weaponModels[i];
        }
        return weaponModel; 
    }
    public void PlayReloadAnimation()
    {
        
            float reloadSpeed = player.weapon.CurrentWeapon().reloadSpeed;
            anim.SetTrigger("Reload");
            anim.SetFloat("ReloadSpeed", reloadSpeed);
            WeaponDecreaseRig();
            
       
    }

    public void PlayWeaponEquipAnimation()
    {

        EquipType equipType = CurrentWeaponModel().equipAnimationType;

        float equipmentSpeed = player.weapon.CurrentWeapon().equipmentSpeed;
        WeaponDecreaseRig();

        anim.SetTrigger("EquipWeapon");
        anim.SetFloat("EquipType", ((float)equipType));
        anim.SetFloat("EquipSpeed", equipmentSpeed);
        
      
    }


    public void SwitchOnCurrentWeaponModel()
    {
        int animationIndex = ((int)CurrentWeaponModel().holdType);
        SwitchAnimationLayers(animationIndex);
        CurrentWeaponModel().gameObject.SetActive(true);

        AttachLeftHand();
    }

    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].gameObject.SetActive(false);
        }
    }


    private void SwitchAnimationLayers(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(layerIndex, 1);
    }


    #region Animation Rigging Methods
    private void AttachLeftHand()
    {
        Transform targetTransform = CurrentWeaponModel().holdPoint;


        leftHandTarget.localPosition = targetTransform.localPosition;
        leftHandTarget.localRotation = targetTransform.localRotation;


    }
    public void IncreaseRigWeight() => shouldAimRigIncreased = true;
    public void IncreaseLeftHandIKWeight() => shouldLeftHandWeightIncreased =true;
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
    private void WeaponDecreaseRig()
    {
        aimRig.weight = 0.1f;
        leftHandIKRig.weight = 0f;
    }
   
    #endregion
}


