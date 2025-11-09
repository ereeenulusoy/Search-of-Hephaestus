using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private Rig leftHandRig;
    private Animator anim;
    [SerializeField] private Transform[] gunTransform;

    [SerializeField] private Transform pistolGun;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform gauntlet;
    [SerializeField] private Transform pipe;
    [SerializeField] private Transform trident;

    private Transform currentGun;
    
    [Header("Left Hand IK")]
    [SerializeField] Transform leftHandTarget;
    

    private void Start()
    {
        SwitchOn(pipe);

        anim = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pipe);
            SwitchAnimationLayers(1);
          
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(pistolGun);
            SwitchAnimationLayers(2);
           
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayers(3);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(gauntlet);
            SwitchAnimationLayers(4);
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(trident);
            SwitchAnimationLayers(5);
           
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
        for (int i = 1; i <anim.layerCount; i++)
        {
        anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(layerIndex, 1);
    }
}
