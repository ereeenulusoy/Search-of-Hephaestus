using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Data/Melee Weapon Data", fileName = "New Weapon Data")]
public class Enemy_MeleeWeaponData : ScriptableObject
{
    public List<MeleeAttackData> attackData;
    //public float turnSpeed = 10;
}
