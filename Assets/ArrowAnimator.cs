using UnityEngine;

public class ArrowAnimator : MonoBehaviour
{
    public float rotationSpeed = 50f; // Okun dönme hızı
    public float hoverAmplitude = 0.2f; // Ne kadar yukarı aşağı salınacağı
    public float hoverSpeed = 1f; // Salınım hızı
    public float startDelay = 0f; // Okun görünür hale gelmeden önceki gecikme

    private Vector3 initialPosition;
    private MeshRenderer meshRenderer;

    void Start()
    {
        initialPosition = transform.position; // Başlangıç pozisyonunu kaydet
        meshRenderer = GetComponent<MeshRenderer>(); // Mesh Renderer'ı al

        // Başlangıçta oku gizle (delay varsa)
        if (startDelay > 0 && meshRenderer != null)
        {
            meshRenderer.enabled = false;
            Invoke("ShowArrow", startDelay); // Belirtilen süre sonra oku göster
        }
    }

    void Update()
    {
        // Okun dönmesi
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Okun yukarı aşağı salınması (hover efekti)
        float newY = initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }

    void ShowArrow()
    {
        if (meshRenderer != null)
        {
            meshRenderer.enabled = true; // Oku görünür yap
        }
    }
}