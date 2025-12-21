using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    private Transform aim;

    [SerializeField] private float weaponRotationSpeed = 12f;

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
            Shoot();
        
        if(Input.GetKeyDown(KeyCode.T))
            currentWeapon.ToggleBurst();
    }
    
    public Weapon CurrentWeapon() => currentWeapon;

    public Weapon BackupWeapon()
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if(weapon != currentWeapon)
                return weapon;
        }
        return null;
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;
    private void EquipWeapon(int i)
    {
        if (i >= weaponSlots.Count)
            return;
        currentWeapon = weaponSlots[i];
        SetWeaponReady(false);
        
        player.visual.PlayWeaponEquipAnimation();

        CameraManager.instance.ChangeCameraDistance(currentWeapon.cameraDistance);
        
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

        player.visual.SwitchOnBackupWeaponModel();
    }

    private IEnumerator BurstFire()
    {
        SetWeaponReady(false);

         for (int i = 1; i <= currentWeapon.bulletsPerShot; i++)
        {
            FireSingleBullet();

            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if (i >= currentWeapon.bulletsPerShot)
                SetWeaponReady(true);   

        }
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
        GetComponentInChildren<Animator>().SetTrigger("Fire");
        
        if (currentWeapon.BurstActivated() == true)
        {

        StartCoroutine(BurstFire());    
        return;

        }
        
        FireSingleBullet();

    }

    private void FireSingleBullet()
    {
        currentWeapon.bulletsInMagazine--;


        GameObject newBullet = ObjectPool.instance.GetBullet();

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulletScript = newBullet.GetComponent<Bullet>();
        bulletScript.BulletSetup(currentWeapon.gunDistance,bulletImpactForce);

        Vector3 bulletsDirection = currentWeapon.ApplySpread(BulletDirection());

        rbNewBullet.mass = REFFERENCED_BULLET_SPEED / bulletSpeed;
        rbNewBullet.velocity = bulletsDirection * bulletSpeed;
    }

    private void ChasingAim()
    {
        
        Vector3 directionToAim = aim.position - weaponHolder.position;
        if (directionToAim == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(directionToAim);

       
        weaponHolder.rotation = Quaternion.Slerp(weaponHolder.rotation, targetRotation, weaponRotationSpeed * Time.deltaTime);


        Vector3 directionGunPoint = aim.position - gunPoint.position;
        if (directionGunPoint != Vector3.zero)
        {
            Quaternion targetGunPointRotation = Quaternion.LookRotation(directionGunPoint);
            gunPoint.rotation = Quaternion.Slerp(gunPoint.rotation, targetGunPointRotation, weaponRotationSpeed * Time.deltaTime);
        }
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
        controls.Character.EquipSlot3.performed += context => EquipWeapon(2);

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };
    }
}
