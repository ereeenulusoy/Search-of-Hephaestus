using UnityEngine;

// Bağlantı noktasının türünü belirler.
public enum SnapPointType { Enter, Exit }

public class SnapPoint : MonoBehaviour
{
    // Unity Inspector'da ayarlanacak nokta türü
    public SnapPointType pointType;

    // Sadece Inspector'da kolaylık sağlamak için (Opsiyonel)
    private void OnValidate()
    {
        gameObject.name = "SnapPoint - " + pointType.ToString();
    }
}