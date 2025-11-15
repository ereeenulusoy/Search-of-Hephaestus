using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    [SerializeField] private Weapon currentWeapon;


    [Header("Bullet")]
    private const float REFFERENCED_BULLET_SPEED = 20f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private Transform weaponHolder;
    private Transform aim;

    [Header("Inventory")]
    [SerializeField] private List<Weapon> weaponSlots;
    // weapon içindeki bilgileri alýp ayrý ayrý weapondaki özellikleri kontrol eder. 0, 1 ...
    public bool isReloadFinished = true;
    public bool isShootFinished = true;
    
    
    private void Start()
    {
        player = GetComponent<Player>();
        HandleInputEvents();

        aim = player.aim.Aim();
        currentWeapon = weaponSlots[0];
    }


    private void Update()
    {
        if (player.movement.isStillDashing || !isReloadFinished || !isShootFinished)
            return;
        ChasingAim();
    }
    
    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i]; 
    }
    private void Shoot()
    {
        if (player.movement.isStillDashing || currentWeapon.ammo <= 0)
        {
            Debug.Log("you cant shoot while dashing. ");
            return;  
        }

        currentWeapon.ammo--;
      
        isShootFinished = false;
        player.visual.ShootPauseRig();

        if (!isReloadFinished)
        {
            isReloadFinished = true;
            player.visual.IncreaseRigWeight();
        }

    
    GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();
      
        rbNewBullet.mass = REFFERENCED_BULLET_SPEED / bulletSpeed;
        rbNewBullet.velocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 5f);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
    private void ChasingAim()
    {
        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);
    }
    public Vector3 BulletDirection()
    {
        Vector3 direction = (aim.position - gunPoint.position).normalized;
       
        if(!player.aim.GetIsAimPricesly() && player.aim.Target() == null)
        direction.y = 0;
        

        return direction;
    }

    public Transform GunPoint() => gunPoint;
    private void HandleInputEvents()
    {
        PlayerControls controls = player.controls;
        controls.Character.Fire.performed += context => Shoot();
        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        //Silahýn Listteki Indexini yaz. 0 -> Þuanda pistolgun'da.
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
    }
    
   
}
