using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Weapon System/Weapon Data" , fileName = "New Weapon Data")]
public class Weapon_Data : ScriptableObject
{
    public string weaponName;

    [Header("Regular Mode")]
    public ShootType shootType;
    public int bulletsPerShot = 1;
    public float fireRate = 5.75f;
    
    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;

    [Header("Burst Mode")]
    public bool burstAvailable;
    public bool burstActive;
    public int burstBulletsPerShot = 4;
    public float burstFireRate = .9f;
    public float burstFireDelay = .13f;

    [Header("Bullet's Spread")]
    public float baseSpread = 1.8f;
    public float maxSpread = 10f;

    public float defaultSpreadIncreaseRate = .6f;
    public float burstSpreadIncreaseRate = .2f;

   

    [Header("Weapon Spesifications")]
    public WeaponType weaponType;
    [Range(0.5f, 3)]
    public float reloadSpeed = 1.1f;
    [Range(0.5f, 3)]
    public float equipmentSpeed = 1.2f;
    [Range(1  , 20)]
    public float gunDistance = 11.8f;
    [Range(15 , 30)]
    public float cameraDistance = 22.5f;
}
