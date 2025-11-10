using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [Header("Intersection check")]
    [SerializeField] private LayerMask intersectionLayer;
    [SerializeField] private Collider[] intersectioncheckcolliders;
    [SerializeField] private Transform intersectionCheckParent;

    // Çakışma Kontrolü
    public bool IntersectionDetected()
    {
        Physics.SyncTransforms();
        foreach (var collider in intersectioncheckcolliders)
        {
            Vector3 extents = collider.bounds.extents;

            Collider[] hitColliders = Physics.OverlapBox(
                collider.bounds.center,
                extents,
                Quaternion.identity,
                intersectionLayer
            );

            foreach (var hit in hitColliders)
            {
                IntersectionCheck intersectionCheck = hit.gameObject.GetComponent<IntersectionCheck>();

                if (intersectionCheck != null)
                {
                    if (hit.transform.root.gameObject != this.gameObject)
                    {
                        Debug.LogWarning("Çakışma tespit edildi: " + this.gameObject.name + " ile " + hit.transform.root.gameObject.name);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // Hizalama ve Konumlandırma
    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();

        // 1. ÖNCE HİZALA (Rotation)
        AlignTo(entrancePoint, targetSnapPoint);

        // 2. SONRA KONUMLANDIR (Position)
        SnapTo(entrancePoint, targetSnapPoint);

        // **DUVAR KALDIRMA ÇAĞRILARI BU NOKTADAN ÇIKARILMIŞTIR**
    }

    // Döndürme Mantığı
    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        Quaternion targetRotation = targetSnapPoint.transform.rotation;
        targetRotation *= Quaternion.Euler(0, 180, 0);
        transform.rotation = targetRotation * Quaternion.Inverse(ownSnapPoint.transform.localRotation);
    }

    // KONUMLANDIRMA MANTIĞI (İç İçe Geçmeyi Önleyen Düzeltme Korunmuştur)
    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Normal Snap Mantığı: Pozisyon offsetini hesapla
        var offset = transform.position - ownSnapPoint.transform.position;
        var newPosition = targetSnapPoint.transform.position + offset;

        // **İÇ İÇE GEÇMEYİ ENGELLEYEN KARŞI-KAYMA DÜZELTMESİ**
        // Bu değeri deneyerek optimize etmeniz gerekir. (0.005f - 0.02f)
        float correctionAmount = 0.05f;

        // SnapPoint'in ters yönünde küçük bir itme uygula (hafif geri çekilme)
        Vector3 pushBack = -targetSnapPoint.transform.forward * correctionAmount;

        // Düzeltmeyi yeni konuma ekle
        newPosition += pushBack;

        transform.position = newPosition;
    }

    // Diğer metodlar
    public SnapPoint GetEntrancePoint() => GetSnapPointOfType(SnapPointType.Enter);
    public SnapPoint GetExitPoint() => GetSnapPointOfType(SnapPointType.Exit);

    private SnapPoint GetSnapPointOfType(SnapPointType pointType)
    {
        SnapPoint[] snapPoints = GetComponentsInChildren<SnapPoint>();
        List<SnapPoint> filteredSnapPoints = new List<SnapPoint>();

        foreach (SnapPoint snapPoint in snapPoints)
        {
            if (snapPoint.pointType == pointType)
            {
                filteredSnapPoints.Add(snapPoint);
            }
        }
        if (filteredSnapPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, filteredSnapPoints.Count);
            return filteredSnapPoints[randomIndex];
        }
        return null;
    }
}