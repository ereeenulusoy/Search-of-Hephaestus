using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerSmoothDoubleJump : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float airControl = 0.4f; // havada yön değiştirme oranı
    public float rotationSmooth = 10f;

    [Header("Jump Settings")]
    public int maxJumps = 2;
    public float jumpHeight = 1.2f;
    public float gravity = -20f;
    public float fallMultiplier = 2f; // düşerken gravity çarpanı
    public float lowJumpMultiplier = 1.5f; // erken bırakıldığında daha kısa zıplama

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.25f;
    public LayerMask groundMask;

    [Header("References")]
    public Camera cam;

    private CharacterController cc;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 smoothMoveDir = Vector3.zero;
    private float yVelocity = 0f;
    private int jumpsLeft;
    private bool isGrounded, wasGrounded;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        if (cam == null) cam = Camera.main;

        // otomatik groundCheck oluştur (yoksa)
        if (groundCheck == null)
        {
            GameObject g = new GameObject("GroundCheck");
            g.transform.SetParent(transform);
            g.transform.localPosition = new Vector3(0f, -0.9f, 0f);
            groundCheck = g.transform;
        }

        jumpsLeft = maxJumps;
    }

    void Update()
    {
        GroundCheck();
        HandleMovement();
        HandleJump();
    }

    void GroundCheck()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (isGrounded && !wasGrounded)
        {
            jumpsLeft = maxJumps; // yere inince hakları yenile
        }

        if (isGrounded && yVelocity < 0f)
            yVelocity = -1f; // yere yapışık tut
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        // kamera yönüne göre hareket
        Vector3 camF = cam.transform.forward; camF.y = 0; camF.Normalize();
        Vector3 camR = cam.transform.right; camR.y = 0; camR.Normalize();
        moveDir = (camF * input.z + camR * input.x).normalized;

        // akıcı geçiş
        float control = isGrounded ? 1f : airControl;
        smoothMoveDir = Vector3.Lerp(smoothMoveDir, moveDir, Time.deltaTime * 8f);

        // hareket et
        cc.Move(smoothMoveDir * moveSpeed * control * Time.deltaTime);

        // yumuşak dönüş
        if (smoothMoveDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(smoothMoveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);
        }
    }

    void HandleJump()
    {
        // zıplama
        if (Input.GetButtonDown("Jump") && jumpsLeft > 0)
        {
            yVelocity = Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight);
            jumpsLeft--;
        }

        // gelişmiş gravity sistemi
        if (yVelocity > 0 && !Input.GetButton("Jump"))
        {
            // erken bırakırsa daha kısa zıplama
            yVelocity += gravity * lowJumpMultiplier * Time.deltaTime;
        }
        else if (yVelocity < 0)
        {
            // düşerken gravity artır
            yVelocity += gravity * fallMultiplier * Time.deltaTime;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        // dikey hareket uygula
        cc.Move(Vector3.up * yVelocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
