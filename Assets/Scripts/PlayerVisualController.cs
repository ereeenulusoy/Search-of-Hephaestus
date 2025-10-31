using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTransform;

    [SerializeField] private Transform pistolGun;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform gauntlet;
    [SerializeField] private Transform pipe;

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOnWeapons(pistolGun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOnWeapons(shotgun);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOnWeapons(gauntlet);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOnWeapons(pipe);
        }
    }

    private void SwitchOnWeapons(Transform whichGun)
    {
        SwitchOffWeapons();
        whichGun.gameObject.SetActive(true);
    }

    private void SwitchOffWeapons()
    {
        for (int i = 0; i < gunTransform.Length; i++)
        { 
        gunTransform[i].gameObject.SetActive(false);   
        }
    }
}
