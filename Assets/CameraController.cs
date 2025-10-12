using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;               // Player (sürükle inspector'a)
    public Vector3 offset = new Vector3(0f, 12f, -12f); // dünya-uzayına göre offset
    public float smoothSpeed = 6f;

    [Header("Fixed Rotation (degrees)")]
    public float fixedXRotation = 50f;     // tilt
    public float fixedYRotation = 0f;      // yaw (y eksenindeki sabit yön)
    public float fixedZRotation = 0f;

    void Start()
    {
        // Kamera sahnede hedef yoksa uyar
        if (target == null) Debug.LogWarning("FixedAngleCamera: target not set.");
        // Başlangıçta sabit rotasyonu uygula
        transform.rotation = Quaternion.Euler(fixedXRotation, fixedYRotation, fixedZRotation);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Hedefin pozisyonuna göre istenen dünya pozisyonunu hesapla
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // Rotasyonu her kare sabitle — böylece oyuncu döndüğünde kamera dönmez
        transform.rotation = Quaternion.Euler(fixedXRotation, fixedYRotation, fixedZRotation);
    }
}
