using UnityEngine;

// Bağlantı noktasının türünü belirler.
public enum SnapPointType { Enter, Exit }

public class SnapPoint : MonoBehaviour
{
    // Unity Inspector'da ayarlanacak nokta türü
    public bool debug;

    public SnapPointType pointType;

    [SerializeField] private GameObject _closeCapObject;

    public void ToggleCaps(bool active)
    {
        if (!debug)
        {
            return;
        }

        if (_closeCapObject == null)
        {
            return;
        }

        _closeCapObject.SetActive(active);

        Debug.Log(_closeCapObject.activeSelf);
    }

    // Sadece Inspector'da kolaylık sağlamak için (Opsiyonel)
    private void OnValidate()
    {
        gameObject.name = "SnapPoint - " + pointType.ToString();
    }
}