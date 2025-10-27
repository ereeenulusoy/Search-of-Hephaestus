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
    private Vector3 movementDirection;
    private float verticalVelocity;

    [Header("Dash")]
    [SerializeField] private Rig aimRig;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField]float fadeInDuration;
    private bool isDashing = false;



   [Header("Aim")]
    [SerializeField] Transform aimTransform;
    [SerializeField] LayerMask aimLayerMask;
    [SerializeField] private float rotationSpeed;
    private Vector3 lookingDirection;
    private Vector3 aimTargetPoint;

    private Vector2 moveInput;
    private Vector2 aimInput;

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        HandleInputEvents();
    }

    private void Update()
    {

        HandleAimTargetPosition();
        if (isDashing)
            return;
        HandleCharacterRotation();
        HandleMovement();
        HandleAnimations();
    }

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
            dashDirection = new Vector3(-transform.forward.x, 0, -transform.forward.z);
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


    private void Shoot()
    {
        animator.SetTrigger("Fire");
    }
    private void HandleAnimations()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);//normalized vektörü 1 yapýyor ve doðrultusunu tutuyor.
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);
        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);
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


    private void HandleAimTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            aimTargetPoint = hitInfo.point;
            aimTransform.position = new Vector3(hitInfo.point.x, transform.position.y + 1.53f, hitInfo.point.z);
        }
    }

    private void HandleCharacterRotation()
    {
        lookingDirection = aimTargetPoint - transform.position;
        lookingDirection.y = 0;
        if (lookingDirection == Vector3.zero) return; 
        Quaternion rotate = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, rotationSpeed);
    }


    private void HandleInputEvents()
    {
        controls = player.controls; //PlayerControls'u sahnede oluþturmaya yarar. Birnevi awake'de instantiate yapýlýr.

        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        //Hangi tuþa basýyorsan performed haldeyken bastýðýn tuþun deðerini context'ten çekiyor.
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;
        //Tuþu býrakýnca yani canceled olunca hýzýmýzý 0lýyoruz.

        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        //Anlýk Ekran uzayýndaki mouse'un konumunu alýyoruz. x =>0 - 1920, y => 0 - 1080
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
        //Ekranda deðilse nullifylýyoruz.
        controls.Character.Dash.performed += context => HandleDash();
    }

}
