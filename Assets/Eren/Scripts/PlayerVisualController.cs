using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTransform;

    [SerializeField] private Transform pistolGun;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform gauntlet;
    [SerializeField] private Transform pipe;
    [SerializeField] private Transform trident;

    private Transform currentGun;
    
    [Header("Left Hand IK")]
    [SerializeField] Transform leftHandTarget;
    // [SerializeField] Transform leftHandHint;
    // Eðer Hint için de ekleyeceksen Target gibi hepsine LeftHand_TargetHintTransform gameObjecti ekle.

    private void Start()
    {
        SwitchOn(pistolGun);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistolGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(shotgun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(gauntlet);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(pipe);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(trident);
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
       // Transform hintTransform = currentGun.GetComponentInChildren<LeftHandTargetHintPosition>().transform;
         // Þuanki silahýn altýndaki lefthandtargetposition scripti bulunan child'ýn konumunu,rotasyonunu,yönünü alýr.

        leftHandTarget.localPosition = targetTransform.localPosition;
        leftHandTarget.localRotation = targetTransform.localRotation;

        //leftHandHint.localPosition = hintTransform.position;
       // leftHandHint.localRotation = hintTransform.localRotation;
    }
}
