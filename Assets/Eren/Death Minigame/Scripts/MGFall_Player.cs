using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGFall_Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    private float xInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxis("Horizontal");
    }

    private void HandleMovement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }
}
