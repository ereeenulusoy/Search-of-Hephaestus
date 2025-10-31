using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private CharacterController characterController;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private float verticalVelocity;
    
    private Vector3 movementDirection;
    private Vector2 moveInput;


    [Header("Dash")]
    [SerializeField] private Rig aimRig;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField]float fadeInDuration;
    private bool isDashing = false;

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        HandleInputEvents();
    }

    private void Update()
    {
        if (isDashing)
            return;
        HandleCharacterRotation();
        HandleMovement();
        HandleAnimations();
    }
   
    private void HandleCharacterRotation()
    {

        Vector3 lookingDirection = player.aim.GetMousePosition() - transform.position;
        lookingDirection.y = 0;
        if (lookingDirection == Vector3.zero) return; 
        Quaternion rotate = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate,rotationSpeed*Time.deltaTime); 
    }


    private void Shoot()
    {
        animator.SetTrigger("Fire");
    }
    private void HandleMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);

        HandleGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * moveSpeed * Time.deltaTime);
        }

    }
  
    #region Dash
    private void HandleDash()
    {
        if (isDashing)
            return;
        StartCoroutine(DashCoroutine());
    }
    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        animator.SetTrigger("doDash");

        
        float timer = 0;

        Vector3 dashDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        if (dashDirection.magnitude < 0.1f)
        {
            dashDirection = new Vector3(-transform.forward.x, 0, -transform.forward.z).normalized;
        }
          
        aimRig.weight = 0f;

        while (timer < dashDuration)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }
        
        float fadeTimer = 0f;

        isDashing = false;

        while (fadeTimer < fadeInDuration)
        {
            aimRig.weight = Mathf.Lerp(0f, 1f, fadeTimer / fadeInDuration);
            fadeTimer += Time.deltaTime;
            yield return null;
        }
        aimRig.weight = 1f;

    }
    #endregion
    private void HandleGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity -= 5f * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
            verticalVelocity = -0.5f;

    }
    private void HandleInputEvents()
    {
        controls = player.controls; //PlayerControls'u sahnede oluþturmaya yarar. Birnevi awake'de instantiate yapýlýr.

        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;
        
        controls.Character.Dash.performed += context => HandleDash();
    }
    private void HandleAnimations()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);//normalized vektörü 1 yapýyor ve doðrultusunu tutuyor.
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);
        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);
    }

}
