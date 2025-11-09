using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [Header("Intersection check")]
    [SerializeField] private LayerMask intersectionLayer;
    [SerializeField] private Collider[] intersectioncheckcolliders;
    [SerializeField] private Transform intersectionCheckParent;

    // Çakışma Kontrolü (İyileştirilmiş)
    public bool IntersectionDetected()
    {
        Physics.SyncTransforms();
        foreach (var collider in intersectioncheckcolliders)
        {
            // Extents (Yarı Boyutlar)
            Vector3 extents = collider.bounds.extents;

            // OverlapBox, belirtilen hacimdeki tüm collider'ları bulur
            Collider[] hitColliders = Physics.OverlapBox(
                collider.bounds.center,
                extents,
                Quaternion.identity, // Rotation olarak Quaternion.identity kullanılması güvenlidir.
                intersectionLayer
            );

            foreach (var hit in hitColliders)
            {
                IntersectionCheck intersectionCheck = hit.gameObject.GetComponent<IntersectionCheck>();

                if (intersectionCheck != null)
                {
                    // Vurulan obje bir IntersectionCheck bileşenine sahipse, bu bir seviye parçasıdır.
                    // Kendi parçamızla çakışmamak için kök objenin farklı olup olmadığını kontrol etmeliyiz.
                    if (hit.transform.root.gameObject != this.gameObject)
                    {
                        // Farklı bir parçayla çakışma tespit edildi.
                        Debug.LogWarning("Çakışma tespit edildi: " + this.gameObject.name + " ile " + hit.transform.root.gameObject.name);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // Sıralama Düzeltildi: Önce Align (Hizalama), sonra Snap (Konumlandırma)
    public void SnapAndAlignPartTo(SnapPoint targetSnapPoint)
    {
        SnapPoint entrancePoint = GetEntrancePoint();

        // 1. ÖNCE HİZALA (Rotation)
        AlignTo(entrancePoint, targetSnapPoint);

        // 2. SONRA KONUMLANDIR (Position)
        SnapTo(entrancePoint, targetSnapPoint);
    }

    // Döndürme Mantığı Düzeltildi
    private void AlignTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // 1. Hedef Snap Point'in dönüşünü al (çıkış yönü).
        Quaternion targetRotation = targetSnapPoint.transform.rotation;

        // 2. 180 derece çevirerek yeni parçanın ona bakmasını sağla (giriş yönü).
        targetRotation *= Quaternion.Euler(0, 180, 0);

        // 3. Yeni parçanın giriş noktasının yerel dönüş farkını hesapla ve uygula.
        // Bu, giriş noktası parçanın pivotunda değilse bile doğru hizalamayı sağlar.
        transform.rotation = targetRotation * Quaternion.Inverse(ownSnapPoint.transform.localRotation);
    }

    // Konumlandırma Mantığı
    private void SnapTo(SnapPoint ownSnapPoint, SnapPoint targetSnapPoint)
    {
        // Dönüş açısı ayarlandığı için, parçanın pivotu ile giriş Snap Point'i arasındaki 
        // güncel mesafeyi (offset) hesapla.
        var offset = transform.position - ownSnapPoint.transform.position;

        // Bu offset'i hedef Snap Point'e uygula.
        var newPosition = targetSnapPoint.transform.position + offset;
        transform.position = newPosition;
    }

    // ... (Diğer metodlar aynı kalır)
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