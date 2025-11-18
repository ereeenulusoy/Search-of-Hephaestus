using UnityEngine;

public enum WeaponType
{
    Pistolgun,
    Shotgun
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
    public float fireRate = 1f; //bullets per second    
    private float lastShootTime;
   
    
    
    [Header("Ammo Details")]
    public int bulletsInMagazine;
    public int magazineCapacity;


    [Range(0.5f,2)]
    public float reloadSpeed = 1f;

    [Range(0.5f, 2)]
    public float equipmentSpeed = 1f;

   
    
    public bool CanShoot()
    {
        if (HaveEnoughBullets() && ReadyToFire())
        {
        bulletsInMagazine--;
        return true;
        }
        return false;
    }

    private bool ReadyToFire()
    {
        if (Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }


    
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