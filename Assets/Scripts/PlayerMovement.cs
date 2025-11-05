using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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

    public Vector2 moveInput { get; private set; }
    private Vector3 movementDirection;

    [Header("Dash")]
    [SerializeField] private Rig aimRig;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float dashCooldown;
    private bool canDash = true;
    public bool isDashing {  get; private set; }
    public bool isStillDashing {  get; private set; }
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

        Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0;
       
        Quaternion rotate = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotationSpeed * Time.deltaTime);
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
        if (!canDash)
            return;

        canDash = false;
        isStillDashing = true;
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

        float fadeTimer = 0f;

        while (timer < dashDuration)
        {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
            aimRig.weight = Mathf.Lerp(0.3f, 0f, timer / fadeOutDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        aimRig.weight = 0f;

        isDashing = false;
        fadeTimer = 0f;

        while (fadeTimer < fadeInDuration)
        {
            aimRig.weight = Mathf.Lerp(0f, 1f, fadeTimer / fadeInDuration);
            fadeTimer += Time.deltaTime;    
            yield return null;
        }
        aimRig.weight = 1f;
       
        isStillDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

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
