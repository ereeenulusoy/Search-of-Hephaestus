using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGFall_Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
   
    private float xInput;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float fallSpeed;

    public bool isCaught = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isCaught)
            return;

        HandleInput();
        HandleMovement();
        HandleAnimations();
    }

    public void FreezePlayer()
    {
        isCaught = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
    private void HandleInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, -fallSpeed);
    }

    private void HandleAnimations()
    {
        anim.SetFloat("xInput", xInput);
    }
}
