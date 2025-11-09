using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{

    private Animator anim;
    [SerializeField] private Transform[] gunTransform;

    [SerializeField] private Transform pistolGun;
    [SerializeField] private Transform shotgun;


    private Transform currentGun;

    [Header("Left Hand IK")]
    [SerializeField] Transform leftHandTarget;


    private void Start()
    {
        SwitchOn(pistolGun);

        anim = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistolGun);
            SwitchAnimationLayers(1);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayers(2);

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
