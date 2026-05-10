using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAres : MonoBehaviour
{
    private Player player;
    [SerializeField] private Animator aresAnim;

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<Player>();

        if (player != null)
        {
            aresAnim.SetTrigger("StartBattle");
        }
    }
}
