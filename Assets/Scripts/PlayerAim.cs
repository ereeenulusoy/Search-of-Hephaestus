using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private Vector2 aimInput;

    [Header("Aim")]
    [SerializeField] private float minCameraDistance = 1.5f;
    [SerializeField] private float maxCameraDistance = 4f;
    [SerializeField] private float aimSensitivity = 5f;

    [SerializeField] Transform aimTransform;
    [SerializeField] LayerMask aimLayerMask;


    private void Start()
    {
        player = GetComponent<Player>();

        AssignInputEvents();
    }

    private void Update()
    {
        aimTransform.position = Vector3.Lerp(aimTransform.position, GetDesiredAimPosition(), aimSensitivity * Time.deltaTime);
    }

    private Vector3 GetDesiredAimPosition()
    {
        float currentMaxCameraDistance = player.movement.moveInput.y < -0.5f ? 2 : maxCameraDistance;

        Vector3 rawMousePosition = GetMousePosition();
        Vector3 aimDirection = rawMousePosition - transform.position; // looking direction'u aldýk.

        aimDirection.Normalize(); // sýnýrlarý aþýyorsa max veya min ile çarpacaðýz. geçmiyorsa da mouse deðerinin kendisiyle.
                                  // Yani bunun büyüklüðü 1 olmalý.

        float distanceToRawPosition = Vector3.Distance(transform.position, rawMousePosition);
        float clampedDistance = Mathf.Clamp(distanceToRawPosition, minCameraDistance, currentMaxCameraDistance);

        Vector3 desiredAimPosition = transform.position + aimDirection * clampedDistance;
        desiredAimPosition.y = transform.position.y + 1.5f;

        return desiredAimPosition;
    }
    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }
}
