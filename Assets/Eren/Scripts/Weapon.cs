public enum WeaponType
{
    Pistolgun,
    Shotgun
}

[System.Serializable]

public class Weapon
{
    public WeaponType weaponType;
    public int ammo;
    public int maxAmmo;
}