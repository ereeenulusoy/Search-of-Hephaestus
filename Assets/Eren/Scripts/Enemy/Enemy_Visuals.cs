using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum Enemy_MeleeWeaponType { OneHand, Throw, Unarmed }
public class Enemy_Visuals : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private Enemy_WeaponModel[] weaponModels;
    [SerializeField] private Enemy_MeleeWeaponType weaponType;
    public GameObject currentWeaponModel {  get; private set; } 


    [Header("Color")]
    [SerializeField] private Texture[] colorTexture;
    [SerializeField] private  SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        weaponModels = GetComponentsInChildren<Enemy_WeaponModel>(true);
    }

    public void EnableWeaponTrail(bool enable)
    {
        Enemy_WeaponModel currentWeaponScript = currentWeaponModel.GetComponent<Enemy_WeaponModel>();
        currentWeaponScript.EnableTrailEffect(enable);
    }
    public void SetupLook()
    {
        SetupRandomColor();
        SetupRandomWeapon();
    }

    public void SetupWeaponType(Enemy_MeleeWeaponType type) => weaponType = type;

    private void SetupRandomWeapon()
    {
        foreach (var weaponModel in weaponModels)
        {
            weaponModel.gameObject.SetActive(false);

        }

        List<Enemy_WeaponModel> filteredWeaponModels = new List<Enemy_WeaponModel>();

        foreach (var weaponModel in weaponModels)
        {
            if (weaponModel.weaponType == weaponType)
                filteredWeaponModels.Add(weaponModel);
        }

        int randomIndex = Random.Range(0, filteredWeaponModels.Count);

        currentWeaponModel = filteredWeaponModels[randomIndex].gameObject;
        currentWeaponModel.gameObject.SetActive(true);
       
        OverrideAnimatorControllerIfCan();

    }

    private void OverrideAnimatorControllerIfCan()
    {
        AnimatorOverrideController overrideController = currentWeaponModel.GetComponent<Enemy_WeaponModel>().overrideController;

        if (overrideController != null)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = overrideController;
        }
    }

    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTexture.Length); 

        Material newMat = new Material(skinnedMeshRenderer.material);
        
        newMat.mainTexture = colorTexture[randomIndex];

        skinnedMeshRenderer.material = newMat;
    }
}
