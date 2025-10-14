using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController characterController;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Vector3 movementDirection;
    private float verticalVelocity;


    [Header("Aim")]
    [SerializeField] Transform aimTransform;
    [SerializeField] LayerMask aimLayerMask;
    private Vector3 lookingDirection;
    
    private Vector2 moveInput;
    private Vector2 aimInput;

    private void Awake()
    {
        controls = new PlayerControls(); //PlayerControls'u sahnede oluþturmaya yarar. Birnevi awake'de instantiate yapýlýr.

        //Baþta atama yapacaðýz. Ardýndan input system performed / cancelled'ý sistem kendisi seçecek ve deðerler ona göre oturacak.

        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        //Hangi tuþa basýyorsan performed haldeyken bastýðýn tuþun deðerini context'ten çekiyor.
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;
        //Tuþu býrakýnca yani canceled olunca hýzýmýzý 0lýyoruz.

        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        //Anlýk Ekran uzayýndaki mouse'un konumunu alýyoruz. x =>0 - 1920, y => 0 - 1080
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
        //Ekranda deðilse nullifylýyoruz.
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        MouseToAim();
       
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

    private void MouseToAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        // var local variable oluþturmayý saðlar. RaycastHit hitInput yazana kadar direkt out var hitInput'la iþi hallederiz.
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            //aimTransform.position = new Vector3(hitInput.point.x, aimTransform.position.y, hitInput.point.z);
            lookingDirection = hitInfo.point - transform.position;
            lookingDirection.y = 0;
            lookingDirection.Normalize(); // Vektörün uzunluðunu artýk almaz sadece yönünü alýr.
           
            transform.forward = lookingDirection;

            aimTransform.position = new Vector3(hitInfo.point.x, aimTransform.position.y ,hitInfo.point.z);
        }
    }

   

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
