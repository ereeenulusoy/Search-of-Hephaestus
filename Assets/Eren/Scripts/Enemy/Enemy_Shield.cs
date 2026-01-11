using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shield : MonoBehaviour
{
    [SerializeField] private int durability;
    private Enemy_Melee enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Melee>();
    }
    public void ReduceDurability()
    {
        durability--;

        if (durability <= 0)
        {
            enemy.anim.SetFloat("ChaseIndex", 0); // enables default chase animation.
            Destroy(gameObject);

        }
    }
}
