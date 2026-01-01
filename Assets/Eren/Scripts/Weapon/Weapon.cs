using UnityEngine;

public enum WeaponType
{
    Pistolgun,
    Shotgun,
    Trident
}

public enum ShootType
{
    Single,
    Auto
}

[System.Serializable]

public class Weapon
{
    public WeaponType weaponType;

    #region Regular Mode Variables
    public ShootType shootType;

    [Space]
    public float defaultFireRate;
    public int bulletsPerShot { get; private set; }
    public float fireRate; //bullets per second    
    private float lastShootTime;
    #endregion

    #region Weapon Spread Variables
    private float baseSpread;
    public float currentSpread;
    private float maximumSpread;

    private float defaultSpreadIncreaseRate; // ÝÞ TAMAMEN BÝTÝNCE DATAYA ÇEKTÝKLERÝNÝ PRIVATE YAP!!!
    public float spreadIncreaseRate;
    private float burstSpreadIncreaseRate;
    private float spreadCooldown = .6f;

    private float lastSpreadUpdateTime;
    #endregion

    #region Burst Mode Variables
    public bool burstAvailable;
    public bool burstActive;

    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay { get; private set; }
    #endregion
  
    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;

    #region Weapon Spesification variables
   
    public float reloadSpeed { get; private set; }
    public float equipmentSpeed { get; private set; }
    public float gunDistance { get; private set; }
    public float cameraDistance { get; private set; }
    #endregion
    // adý boþuna constructor deðil. herhangi bir dönüþ tipi yok sadece varlýk oluþturur.
    public Weapon(Weapon_Data weaponData)
    {   
        bulletsInMagazine = weaponData.bulletsInMagazine;
        magazineCapacity = weaponData.magazineCapacity;

        fireRate = weaponData.fireRate;
        weaponType = weaponData.weaponType;
        shootType = weaponData.shootType;
        bulletsPerShot = weaponData.bulletsPerShot;

        burstActive = weaponData.burstActive;
        burstAvailable = weaponData.burstAvailable;
        burstBulletsPerShot = weaponData.burstBulletsPerShot;
        burstFireRate = weaponData.burstFireRate;
        burstFireDelay = weaponData.burstFireDelay;

        baseSpread = weaponData.baseSpread;
        maximumSpread = weaponData.maxSpread;
        currentSpread = baseSpread;

        defaultSpreadIncreaseRate = weaponData.defaultSpreadIncreaseRate;
        burstSpreadIncreaseRate = weaponData.burstSpreadIncreaseRate;
        spreadIncreaseRate = defaultSpreadIncreaseRate;

        reloadSpeed = weaponData.reloadSpeed;
        equipmentSpeed = weaponData.equipmentSpeed;
        gunDistance = weaponData.gunDistance;
        cameraDistance = weaponData.cameraDistance;


        defaultFireRate = fireRate;
    }

    #region Burst Methods

    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }

        return burstActive;
    }

    public void ToggleBurst()
    {
        if (!burstAvailable)
            return;

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
            spreadIncreaseRate = burstSpreadIncreaseRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
            spreadIncreaseRate = defaultSpreadIncreaseRate;
        }
    }


    #endregion
    public bool CanShoot() => HaveEnoughBullets() && ReadyToFire();
    
    
    private bool ReadyToFire()
    {
        if (Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }

    #region Spread Methods
    public Vector3 ApplySpread(Vector3 originalDirection)
    {

        UpdateSpread(); 

        float randomizedValue = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }
    
    private void UpdateSpread()
    {
        if (Time.time >= lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();

            lastSpreadUpdateTime = Time.time;
    }

    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }
    #endregion

    
    #region Reload Methods
    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
        {
            return false;
        }
        return true;
    }
    public void RefillBullets()
    {
        bulletsInMagazine = magazineCapacity;
    }
    private bool HaveEnoughBullets() => bulletsInMagazine > 0;
   
    #endregion

}