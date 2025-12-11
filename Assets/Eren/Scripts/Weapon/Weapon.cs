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

    [Space]
    [Header("Shooting Specifics")]
    public ShootType shootType;
    public float defaultFireRate;
    public int bulletsPerShot;
    public float fireRate = 1f; //bullets per second    
    private float lastShootTime;

    [Header("Bullet Spread")]
    public float baseSpread;
    public float currentSpread = 2;
    public float maximumSpread = 3;

    public float defaultSpreadIncreaseRate = .2f;
    public float spreadIncreaseRate = .3f;
    public float burstSpreadIncreaseRate = .2f;
    public float spreadCooldown;

    private float lastSpreadUpdateTime;

    [Header("Burst fire")]
    public bool burstModeAvailable;
    public bool burstActive;

    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = 0.1f;
    
    [Header("Ammo Details")]
    public int bulletsInMagazine;
    public int magazineCapacity;


    [Range(0.5f,2)]
    public float reloadSpeed = 1f;

    [Range(0.5f, 2)]
    public float equipmentSpeed = 1f;



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
        if (!burstModeAvailable)
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