using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private Vector2 aimInput;


    [Header("Aim")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;

    [SerializeField] private bool isAimingPricesly;
    [SerializeField] private bool isLockingToTarget;
    [Space]

    [Header("Camera")]
    [SerializeField] Transform cameraFollowTarget;
    [SerializeField] private float minCameraDistance = 1.5f;
    [SerializeField] private float maxCameraDistance = 3f;
    [SerializeField] private float cameraSensitivity = 9f;



    RaycastHit lastHitInfo;
    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isAimingPricesly = !isAimingPricesly;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            isLockingToTarget = !isLockingToTarget;
        }

        UpdateAimPosition();
        UpdateCameraPosition();
    }

    

    public Transform Target()
    {
        Transform target = null;

        if (GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }

        return target;
    }
    private void UpdateAimPosition()
    {
        Transform target = Target();

        if (target != null && isLockingToTarget)
        {
            if(target.GetComponent<Renderer>() != null) 
             aim.position = target.GetComponent<Renderer>().bounds.center;
            else
             aim.position = target.position;
            

            return;
        }

        aim.position = GetMouseHitInfo().point;

        if (!isAimingPricesly)
            aim.position = new Vector3(aim.position.x, transform.position.y + 1.5f, aim.position.z);
    }

    #region Camera
    private void UpdateCameraPosition()
    {
        cameraFollowTarget.position = Vector3.Lerp(cameraFollowTarget.position, GetDesiredCameraPosition(), cameraSensitivity * Time.deltaTime);
    }
    private Vector3 GetDesiredCameraPosition()
    {
        float currentMaxCameraDistance = player.movement.moveInput.y < -0.5f ? 2 : maxCameraDistance;

        Vector3 rawCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (rawCameraPosition - transform.position).normalized;


        float distanceToRawCameraPosition = Vector3.Distance(transform.position, rawCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToRawCameraPosition, minCameraDistance, currentMaxCameraDistance);

        Vector3 desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 1.5f;

        return desiredCameraPosition;
    }
    #endregion
    public RaycastHit GetMouseHitInfo()
    {

        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastHitInfo = hitInfo;
            return hitInfo;
        }
        return lastHitInfo;
    }

    public Transform Aim() => aim;
    public bool GetIsAimPricesly() => isAimingPricesly;
    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
