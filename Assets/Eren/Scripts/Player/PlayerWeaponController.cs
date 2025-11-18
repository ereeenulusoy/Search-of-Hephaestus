using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    private Transform aim;


    [Header("Bullet")]
    [SerializeField] private float bulletImpactForce = 100f;
    private const float REFFERENCED_BULLET_SPEED = 20f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private Transform weaponHolder;



    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;
    // weapon içindeki bilgileri alýp ayrý ayrý weapondaki özellikleri kontrol eder. 0, 1 ...
   
    [SerializeField] private Weapon currentWeapon;
    

    private bool weaponReady;
    private bool isShooting;
    
    private void Start()
    {
        player = GetComponent<Player>();
        HandleInputEvents();

        aim = player.aim.Aim();
        
        Invoke("EquipStartingWeapon",.1f);
    }

    private void EquipStartingWeapon() => EquipWeapon(0);
  
    private void Update()
    {
        //eðer hissiyat istersen shoot için de ayrý bool ve a.event yap.
        if (player.movement.isStillDashing || !WeaponReady())
            return;
        
        ChasingAim();

        if (isShooting)
        {
            Shoot();
        }
    }
    
    public Weapon CurrentWeapon() => currentWeapon;
    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i];
        SetWeaponReady(false);
        player.visual.SwitchOffWeaponModels();
        player.visual.PlayWeaponEquipAnimation();
        
    }

    private void Reload()
    {
        if (WeaponReady())
        {
        SetWeaponReady(false);
        player.visual.PlayReloadAnimation();
        }
    }
    public void SetWeaponReady(bool ready) => weaponReady = ready;
    public bool WeaponReady() => weaponReady;

    public void PickUpWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots)
           return;

        weaponSlots.Add(newWeapon);
    }
    private void Shoot()
    {
        if (player.movement.isStillDashing || !currentWeapon.CanShoot() || !WeaponReady())
        {
            return;  
        }
        if (currentWeapon.shootType == ShootType.Single)
        {
            isShooting = false;
        }
        GameObject newBullet = ObjectPool.instance.GetBullet();
        //GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetup(bulletImpactForce);
      
        rbNewBullet.mass = REFFERENCED_BULLET_SPEED / bulletSpeed;
        rbNewBullet.velocity = BulletDirection() * bulletSpeed;

       
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
        
        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        
        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };
    }
}
