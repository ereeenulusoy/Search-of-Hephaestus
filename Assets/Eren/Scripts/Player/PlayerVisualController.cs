using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Networking;

public class PlayerVisualController : MonoBehaviour
{
    private Player player;
    
    [Header ("Rig")]
    [SerializeField] Rig aimRig;
    [SerializeField] private float aimRigIncreaseSpeed;
    [SerializeField] private float aimRigDecreaseSpeed;
    private bool shouldAimRigIncreased;
    private bool shouldAimRigDecreased;

    [Header("Left Hand IK")]
    [SerializeField] Transform leftHandTarget;
    [SerializeField] Rig leftHandIKRig;
    [SerializeField] float leftHandIK_IncreaseSpeed;
    [SerializeField] float leftHandIK_DecreaseSpeed;
    private bool shouldLeftHandWeightIncreased;
    private bool shouldLeftHandIKDecreased;


    private Animator anim;

    [SerializeField] private WeaponModel[] weaponModels;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;

    


    private void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
        backupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }
    private void Update()
    {
        UpdateAimRigDecrease();
        UpdateLeftHandIKDecrease();

        UpdateAimRigIncrease();
        UpdateLeftHandIKIncrease();
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

        SwitchOffWeaponModels();
        SwitchOffBackupWeaponModels();
        
        if(!player.weapon.HasOnlyOneWeapon())
        SwitchOnBackupWeaponModel();
        

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

    public void SwitchOnBackupWeaponModel()
    {
        WeaponType weaponType = player.weapon.BackupWeapon().weaponType;

        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            if (backupWeaponModel.weaponType == weaponType)
            {
                backupWeaponModel.gameObject.SetActive(true);
            }
        }
    }
    private void SwitchOffBackupWeaponModels()
    {
        foreach (BackupWeaponModel backupWeaponModel in backupWeaponModels)
        {
            backupWeaponModel.gameObject.SetActive(false);
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

    public void DecreaseRigWeight() => shouldAimRigDecreased = true;
    public void DecreaseLeftHandIKWeight() => shouldLeftHandIKDecreased = true;

    private void UpdateAimRigIncrease()
    {
        if (shouldAimRigIncreased)
        {
            aimRig.weight += aimRigIncreaseSpeed * Time.deltaTime;
           

            if (aimRig.weight >= 1f)
                shouldAimRigIncreased = false;
            
        }
    }
    private void UpdateAimRigDecrease()
    {
        if (shouldAimRigDecreased)
        {
            aimRig.weight -= aimRigDecreaseSpeed * Time.deltaTime;

            if (aimRig.weight <= 0.1f)
            {
                aimRig.weight = 0.1f;
                shouldAimRigDecreased = false;
            }
        }
    }
    private void UpdateLeftHandIKIncrease()
    {
        if (shouldLeftHandWeightIncreased)
        {
            leftHandIKRig.weight += leftHandIK_IncreaseSpeed * Time.deltaTime;

            if (leftHandIKRig.weight >= 1f)
                shouldLeftHandWeightIncreased = false;
        }
    }
    private void UpdateLeftHandIKDecrease()
    {
        if (shouldLeftHandIKDecreased)
        {
            leftHandIKRig.weight -= leftHandIK_DecreaseSpeed * Time.deltaTime;

            if (leftHandIKRig.weight <= 0f)
            {
                leftHandIKRig.weight = 0f;
                shouldLeftHandIKDecreased = false;
            }
        }
    }
    private void WeaponDecreaseRig()
    {
        DecreaseRigWeight();
        DecreaseLeftHandIKWeight();
    }
   
    #endregion
}


