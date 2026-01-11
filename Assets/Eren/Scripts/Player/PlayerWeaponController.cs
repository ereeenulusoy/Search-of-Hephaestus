using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    private const float REFFERENCED_BULLET_SPEED = 20f;
    private Transform aim;

    [SerializeField] private float weaponRotationSpeed = 12f;

    [Header("Bullet")]
    [SerializeField] private float bulletImpactForce = 100f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private Transform weaponHolder;


    [Header("Inventory")]
    [SerializeField] private Weapon_Data defaultWeaponData;
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;
    // weapon içindeki bilgileri alýp ayrý ayrý weapondaki özellikleri kontrol eder. 0, 1 ...
   
    

    private bool weaponReady;
    private bool isShooting;
    
    private void Start()
    {
        player = GetComponent<Player>();
        HandleInputEvents();

        aim = player.aim.Aim();
        
        Invoke(nameof(EquipStartingWeapon),.1f);
    }

    private void EquipStartingWeapon() 
    {
        weaponSlots[0] = new Weapon(defaultWeaponData);    
        EquipWeapon(0);
    }
  
    private void Update()
    {
        //eðer hissiyat istersen shoot için de ayrý bool ve a.event yap.
        if (player.movement.isStillDashing || !WeaponReady())
            return;
        
        ChasingAim();

        if (isShooting)
            Shoot();
        
    }
    
    public Weapon CurrentWeapon() => currentWeapon;

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;
    public Weapon WeaponInSlots(WeaponType weaponType)
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if(weapon.weaponType == weaponType)
                return weapon;
        }
        return null;
    }
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

        player.visual.CurrentWeaponModel().reloadSFX.Play();

        }
    }
    public void SetWeaponReady(bool ready) => weaponReady = ready;
    public bool WeaponReady() => weaponReady;

    public void PickUpWeapon(Weapon_Data newWeaponData)
    {
        if (weaponSlots.Count >= maxSlots)
           return;
       
        Weapon newWeapon = new Weapon(newWeaponData);
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
        TriggerEnemyDodge();


    }

    private void FireSingleBullet()
    {
        currentWeapon.bulletsInMagazine--;


        player.visual.CurrentWeaponModel().fireSFX.Play();

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab, GunPoint());

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
        Vector3 direction = (aim.position - GunPoint().position).normalized;
       
        if(!player.aim.GetIsAimPricesly() && player.aim.Target() == null)
        direction.y = 0;
        

        return direction;
    }

    public Transform GunPoint() => gunPoint;

    private void TriggerEnemyDodge()
    {
        Vector3 rayOrigin = GunPoint().position;
        Vector3 rayDirection = BulletDirection();

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, Mathf.Infinity))
        {
            Enemy_Melee enemy_Melee = hit.collider.gameObject.GetComponentInParent<Enemy_Melee>();
            //Ragdollar kolda vesayre yani childda. O yüzden parenta bakýlmalý.

            if (enemy_Melee != null)
            {
                enemy_Melee.ActivateDodgeRoll();
            }
        }
    }
    private void HandleInputEvents()
    {
        PlayerControls controls = player.controls;
        
        controls.Character.Fire.performed += context => isShooting = true;
        controls.Character.Fire.canceled += context => isShooting = false;

        controls.Character.EquipSlot1.performed += context => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += context => EquipWeapon(1);
        controls.Character.EquipSlot3.performed += context => EquipWeapon(2);
        controls.Character.EquipSlot4.performed += context => EquipWeapon(3);
        controls.Character.EquipSlot5.performed += context => EquipWeapon(4);

        controls.Character.ToggleWeaponMode.performed += context => currentWeapon.ToggleBurst();

        controls.Character.Reload.performed += context =>
        {
            if (currentWeapon.CanReload() && WeaponReady())
            {
                Reload();
            }
        };
    }
}
